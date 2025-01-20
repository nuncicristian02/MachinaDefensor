using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : SupportTowerBase
{
    public GameObject projectilePrefab;

    public float HealingPoints;
    public override void Effect(GameObject target)
    {
        if (target != null)
        {
            TowerBase targetTower = target.GetComponent<TowerBase>();

            HealTarget(targetTower);
        }        
    }
    
    private void HealTarget(TowerBase targetTower)
    {
        targetTower.Heal(HealingPoints);
    }
}
