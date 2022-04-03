using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : CollidableObject
{
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
