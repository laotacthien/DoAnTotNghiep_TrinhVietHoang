using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Patrol Settings")]
    public float patrolSpeed = 1.5f;
    public float patrolDistance = 3f;

    private Rigidbody2D rb;
    public Vector2 startPosition;  // dùng cho cả EnemyAI
    public bool movingRight = true;   // dùng cho cả EnemyAI

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    public void Patrol()
    {
        float currentX = transform.position.x;
        Vector2 velocity = rb.linearVelocity;

        if (movingRight)
        {
            if (currentX >= startPosition.x + patrolDistance)
            {
                movingRight = false;
                Flip();
            }
            velocity.x = patrolSpeed;
        }
        else
        {
            if (currentX <= startPosition.x - patrolDistance)
            {
                movingRight = true;
                Flip();
            }
            velocity.x = -patrolSpeed;
        }

        rb.linearVelocity = velocity;
    }
    public void StopPatrol()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
