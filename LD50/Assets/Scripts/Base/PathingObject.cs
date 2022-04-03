using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[
    RequireComponent(typeof(Seeker)),
    RequireComponent(typeof(Rigidbody2D))
]
public class PathingObject : MonoBehaviour
{
    public Transform mainTarget;
    public Transform[] targets;
    private float targetCloseDistance = 0.8f;

    private int currentWaypoint = 0;
    public float nextWaypointDistance = 3f;

    private Path path;
    private bool reachedEndOfPath = false;
    public float updatePathInterval = 0.5f;

    private Seeker seeker;
    protected Rigidbody2D rb;

    public float speed = 200f;
    protected Vector2 direction;
    private Vector2 force;

    protected virtual void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, updatePathInterval);
    }

    protected virtual void FixedUpdate()
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
    }

    protected virtual void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            var targetPosition = GetTargetPosition();
            seeker.StartPath(rb.position, targetPosition, OnPathComplete);
        }
    }

    protected virtual Vector2 GetTargetPosition()
    {
        Transform rtn = mainTarget;
        float distanceFromTarget = Vector2.Distance(rb.transform.position, rtn.position);

        for (var i = 0; i < targets.Length; i++)
        {
            float distanceFromOtherTarget = Vector2.Distance(rb.transform.position, targets[i].position);

            if (distanceFromTarget > distanceFromOtherTarget)
            {
                rtn = targets[i];
                distanceFromTarget = distanceFromOtherTarget;
            }
        }

        if (distanceFromTarget < targetCloseDistance)
        {
            onCloseToTarget();
        }

        return rtn.position;
    }

    protected virtual void onCloseToTarget()
    {
        Debug.Log(name + " is close to target");
    }
}
