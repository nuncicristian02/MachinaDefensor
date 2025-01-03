using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class TowerBase : PositionableTower
    {
        public abstract void Attack(GameObject target);

        public float AttackCooldown = 5;
        protected float LastAttack { get; set; } = 0;
        public float ProjectileSpeed = 5f;
        public float ProjectileDamage = 2f;
        public float Price;

        public int Level;

        public List<int> UpdatePrices = new List<int>()
        {
            50, 75, 100
        };

        public int UpdatePrice
        {
            get { return _updatePrice; }
            set 
            {
                _updatePrice = value; 
                transform.GetComponentInChildren<UpdateTowerSpriteButton>().UpdatePriceText(value);
            }
        }

        private int _updatePrice;

        protected override void Awake()
        {
            base.Awake();
            Level = 0;
            UpdatePrice = UpdatePrices[Level];
        }
        private void Update()
        {
            if (!isPositioned)
            {
                return;
            }

            var target = GetTarget();

            if (Time.time - LastAttack >= AttackCooldown && target)
            {
                LastAttack = Time.time;
                Attack(target);
            }
        }

        private GameObject GetTarget()
        {
            var target = FindClosestEnemyToPlayerBaseInTowerRadius();

            return target ? target : null;
        }

        private GameObject FindClosestEnemyToPlayerBaseInTowerRadius()
        {
            GameObject playerBase = GameObject.FindGameObjectWithTag("PlayerBase");

            if (playerBase == null)
            {
                Debug.LogWarning("PlayerBase non trovato!");
                return null;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies.Where(x => TowerRadiusObject.GetComponent<Collider2D>().IsTouching(x.GetComponent<Collider2D>())))
            {
                float distance = Vector3.Distance(playerBase.transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        public override bool VerifyDraggable()
        {
            return GameManager.Instance.Player.Money >= Price && !isTouchingTowers;
        }       

        protected override void OnDropTower()
        {
            GameManager.Instance.Player.Money -= Price;
        }

        public void OnDeleteTower()
        {
            GameManager.Instance.Player.Money += Price/2 + (float)Math.Floor(UpdatePrices.TakeWhile(x => x < UpdatePrice).Sum(x => x / 2.0f));
            Destroy(gameObject);
        }

        public void OnUpgradeTower()
        {
            
            if (GameManager.Instance.Player.Money >= UpdatePrice)
            {
                GameManager.Instance.Player.Money -= UpdatePrice;

                AddLevel();

                VisualizeTowerOptionsAndRadius();
            }
        }

        private bool VerifyLevel()
        {
            return Level < UpdatePrices.Count - 1;
        }

        private void AddLevel()
        {
            if (!VerifyLevel())
            {
                transform.GetComponentInChildren<UpdateTowerSpriteButton>().gameObject.SetActive(false);
            }
            else
            {
                Level += 1;
                UpdatePrice = UpdatePrices[Level];
            }
            ProjectileDamage += 1;
        }
    }    
}
