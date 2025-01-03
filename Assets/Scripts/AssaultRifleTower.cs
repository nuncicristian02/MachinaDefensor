using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleTower : TowerBase
{
    public GameObject projectilePrefab; 
    public override void Attack(GameObject target)
    {
        if (target != null)
        {
            LaunchProjectile(target);
        }
    }
    
    void LaunchProjectile(GameObject target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.Initialize(target.transform, ProjectileSpeed, ProjectileDamage);
        }
    }
    
}
