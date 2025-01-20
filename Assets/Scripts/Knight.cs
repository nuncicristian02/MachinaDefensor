using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Knight : AttackTowerBase
{
    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void Effect(GameObject target)
    {
        if (target != null)
        {
            Swing(target);
        }
    }

    private void Swing(GameObject target)
    {
        animator.SetTrigger("Swing");
    }
    
}
