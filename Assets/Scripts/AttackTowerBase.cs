using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class AttackTowerBase : TowerBase
    {
        public float Damage;
        protected override GameObject GetTarget()
        {
            return FindClosestEnemyToPlayerBaseInTowerRadius();
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
    }
}
