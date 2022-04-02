using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HeroController : MonoBehaviour
{
    // A Star Pathfinding
    public Transform target;

    // public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    // End A Star Pathfinding

    private float speed = 1f;
    private float friction = 0.9f;
    private float max_speed = 4f;

    Vector2 direction;
    Vector2 velocity;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;

    void Start()
    {
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.1f);

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

    // Update is called once per frame
    void FixedUpdate()
    {
        // direction.x = Input.GetAxisRaw("Horizontal");
        // direction.y = Input.GetAxisRaw("Vertical");
        // direction = direction.normalized;

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

        animate();
        movement();

        rb.velocity = velocity;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // void FixedUpdate()
    // {
    // animate();
    // movement();

    // rb.velocity = velocity;
    // }

    void movement()
    {
        velocity.x += direction.x;
        velocity.y += direction.y;
        velocity = velocity * speed * friction; // Time.fixedDeltaTime;

        velocity.x = Mathf.Max(-max_speed, Mathf.Min(velocity.x, max_speed));
        velocity.y = Mathf.Max(-max_speed, Mathf.Min(velocity.y, max_speed));
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
