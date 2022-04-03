using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HeroController : PathingObject
{
    public SpriteRenderer sr;
    public Animator animator;
    public GameObject sword;

    private SwordController swordController;

    protected override void Start()
    {
        base.Start();

        swordController = sword.GetComponent<SwordController>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Animate();
    }

    protected override void onCloseToTarget()
    {
        Attack();
    }

    void Animate()
    {
        bool isMoving = direction != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void Attack()
    {
        swordController.Attack();
    }
}
