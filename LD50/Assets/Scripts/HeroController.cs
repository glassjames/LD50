using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HeroController : MonoBehaviour
{
    public Transform mainTarget;
    public Transform target;

    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;

    private float speed = 200f;
    Vector2 direction;
    Vector2 force;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            float distanceFromMain = Vector2.Distance(rb.transform.position, mainTarget.position);
            float distanceFromEnemy = Vector2.Distance(rb.transform.position, target.position);

            var targetPosition = distanceFromEnemy < distanceFromMain && distanceFromEnemy < 2 ? target.position : mainTarget.position;

            seeker.StartPath(rb.position, targetPosition, OnPathComplete);
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

    // Update is called once per frame
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

        animate();

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void animate()
    {
        bool isMoving = direction != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            if (direction.x < 0 && !sr.flipX)
            {
                sr.flipX = true;
            }
            else if (direction.x > 0 && sr.flipX)
            {
                sr.flipX = false;
            }
        }
    }
}
