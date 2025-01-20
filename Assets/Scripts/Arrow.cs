using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Throwable
{
    protected override void DoEffect(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }
}
