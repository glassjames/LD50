using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;

    private float speed = 300f;

    Vector2 direction;
    Vector2 force;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    // public Animator animator;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.3f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.deltaTime;

        // animate();

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // void animate()
    // {
    //     bool isMoving = direction != Vector2.zero;
    //     animator.SetBool("isMoving", isMoving);

    //     if (isMoving)
    //     {
    //         if (direction.x < 0 && !sr.flipX)
    //         {
    //             sr.flipX = true;
    //         }
    //         else if (direction.x > 0 && sr.flipX)
    //         {
    //             sr.flipX = false;
    //         }
    //     }
    // }
}
