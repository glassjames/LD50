using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : PathingObject
{
    public SpriteRenderer sr;
    // public Animator animator;

    protected override void Start()
    {
        base.Start();

        speed = 300f;
        updatePathInterval = 0.3f;

        mainTarget = GameObject.Find("hero").transform;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
