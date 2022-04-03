using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : CollidableObject
{
    // public int damagePoint = 1;
    // public float pushForce = 2.0f;

    // public int lvl = 0;
    // public float cooldown = 0.5f;
    // private float lastSwing;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
