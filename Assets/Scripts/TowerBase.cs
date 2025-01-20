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
        public float _life = 100;
        public float OriginalLife;

        private GameObject TowerLifeGameObject;
        private SpriteRenderer lifeBarSpriteRenderer;
        public float Life
        {
            get
            {
                return _life;
            }
            set
            {
                _life = value;
                ManageDeath();
                ManageLifeBar();
            }
        }

        public void Heal(float healingPoints)
        {
            Life += healingPoints;
        }

        private void ManageLifeBar()
        {
            if (!TowerLifeGameObject.activeSelf && _life < OriginalLife)
            {
                TowerLifeGameObject.SetActive(true);
            }

            if (_life > 0)
            {
                var newXLocalScale = _life / OriginalLife;

                var lifeBarLocalScale = lifeBarSpriteRenderer.gameObject.transform.localScale;

                var newLocalScale = new Vector3(newXLocalScale, lifeBarLocalScale.y, lifeBarLocalScale.z);

                lifeBarSpriteRenderer.gameObject.transform.localScale = newLocalScale;
            }
        }

        private void ManageDeath()
        {
            if (_life > 0) return;

            GameObject.Destroy(gameObject);
        }

        public abstract void Effect(GameObject target);

        public float EffectCooldown = 5;
        protected float LastEffect { get; set; } = 0;
        
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

            TowerLifeGameObject = transform.GetChild(1).gameObject;
            lifeBarSpriteRenderer = TowerLifeGameObject?.transform.GetChild(0).GetComponent<SpriteRenderer>();
            OriginalLife = _life;
        }
        private void Update()
        {
            if (!isPositioned)
            {
                return;
            }

            var target = GetTarget();

            if (target)
            {
                RotateTowardsTarget(target.transform.position);
            }

            if (Time.time - LastEffect >= EffectCooldown && target)
            {
                LastEffect = Time.time;
                Effect(target);
            }
        }

        private void RotateTowardsTarget(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        protected abstract GameObject GetTarget();

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
        }
    }    
}
