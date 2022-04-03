using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HeroController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    public GameObject sword;

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

        reachedEndOfPath = currentWaypoint >= path.vectorPath.Count;

        if (!reachedEndOfPath)
        {
            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }

        Animate();
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
        var swordController = sword.GetComponent<SwordController>();
        swordController.Attack();
    }
}
