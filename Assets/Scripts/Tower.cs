using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class TowerBase : PositionableTower
    {
        public abstract void Attack();

        public float AttackCooldown = 5;
        public float Range;
        protected float LastAttack { get; set; } = 0;
        public float ProjectileSpeed = 5f;
        public float ProjectileDamage = 2f;
        public float Price;
        public float UpdatePrice;

        private void Update()
        {
            if (!isPositioned)
            {
                return;
            }

            if (Time.time - LastAttack >= AttackCooldown && VerifyRange())
            {
                LastAttack = Time.time;
                Attack();
            }
        }

        private bool VerifyRange() => true;

        public override bool VerifyDraggable()
        {
            return GameManager.Instance.Player.Money >= Price;
        }

        protected override void OnDropTower()
        {
            GameManager.Instance.Player.Money -= Price;
        }

        public void OnDeleteTower()
        {
            GameManager.Instance.Player.Money += Price;
            Destroy(gameObject);
        }

        public void OnUpgradeTower()
        {
            if (GameManager.Instance.Player.Money >= UpdatePrice)
            {
                GameManager.Instance.Player.Money -= UpdatePrice;
                ProjectileDamage += 1;
                VisualizeTowerOptions();
            }
        }
    }    
}
