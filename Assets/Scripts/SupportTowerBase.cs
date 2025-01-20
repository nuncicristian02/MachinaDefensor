using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts
{
    public abstract class SupportTowerBase : TowerBase
    {
        protected override GameObject GetTarget()
        {
            return FindFriendlyUnitInRadius();
        }

        private GameObject FindFriendlyUnitInRadius()
        {
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
            foreach (GameObject tower in towers)
            {
                if (TowerRadiusObject.GetComponent<Collider2D>().IsTouching(tower.GetComponent<Collider2D>()))
                {
                    TowerBase targetTower = tower.GetComponent<TowerBase>();

                    if (targetTower.Life < targetTower.OriginalLife)
                    {
                        return tower;
                    }

                }
            }

            return null;
        }
    }
}
