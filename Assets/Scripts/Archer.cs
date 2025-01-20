using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : AttackTowerBase
{
    public GameObject arrowPrefab;

    public float ArrowSpeed = 5f;
    public override void Effect(GameObject target)
    {
        if (target != null)
        {
            LaunchProjectile(target);
        }
    }
    
    void LaunchProjectile(GameObject target)
    {
        GameObject arrowObj = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Throwable arrow = arrowObj.GetComponent<Throwable>();

        if (arrow != null)
        {
            arrow.Initialize(target.transform, ArrowSpeed, Damage);
        }
    }
    
}
