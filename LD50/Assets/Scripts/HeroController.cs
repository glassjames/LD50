using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    private float speed = 1f;
    private float friction = 0.9f;
    private float max_speed = 4f;

    Vector2 direction;
    Vector2 velocity;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        direction = direction.normalized;
    }

    void FixedUpdate()
    {
        animate();
        movement();

        rb.velocity = velocity;
    }

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
