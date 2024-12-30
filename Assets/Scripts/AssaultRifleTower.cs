using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleTower : TowerBase
{
    public GameObject projectilePrefab; 
    public override void Attack()
    {
        GameObject target = FindClosestEnemyToPlayerBase();

        if (target != null)
        {
            LaunchProjectile(target);
        }
    }

    private GameObject FindClosestEnemyToPlayerBase()
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

        foreach (GameObject enemy in enemies)
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
