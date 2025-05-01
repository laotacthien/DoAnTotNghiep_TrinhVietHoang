using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    [Header("Chase Settings")]
    public float chaseSpeed = 3f;
    public float stoppingDistance = 1f;

    public float lostTargetTime = 2f;


    private Transform player;
    private Rigidbody2D rb;
    private bool isChasing = false;

    private EnemyMovement enemyMovement;
    private float lastSeenTime;
    public bool isAttacking = false;
    public bool isKnockback = false;

    public Transform Player => player;
    public bool IsChasing => isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyMovement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        //if (player != null)
        //{
        //    // Kiểm tra liên tục nếu player đã ra khỏi phạm vi
        //    float currentDistance = Vector2.Distance(transform.position, player.position);
        //    if (currentDistance > detectionRadius || !HasLineOfSight(player))
        //    {
        //        player = null;
        //        isChasing = false;
        //        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Dừng di chuyển
        //        return;
        //    }

        //    HandleChaseBehavior();
        //}
        //else
        //{
        //    FindPlayer();
        //}
        EnemyAi();
    }
    private void EnemyAi()
    {
        if (isKnockback) return; // Dừng AI nếu đang bị đánh

        if (player != null)
        {
            float currentDistance = Vector2.Distance(transform.position, player.position);
            bool canSeePlayer = currentDistance <= detectionRadius && HasLineOfSight(player);

            if (canSeePlayer)
            {
                lastSeenTime = Time.time;
                HandleChaseBehavior();
            }
            else if (Time.time - lastSeenTime < lostTargetTime)
            {
                HandleChaseBehavior();
            }
            else
            {
                player = null;
                isChasing = false;

                //điều chỉnh hướng và chuyển về trạng thái Patrol
                if (transform.localScale.x > 0f) enemyMovement.movingRight = true;
                else enemyMovement.movingRight = false;
                enemyMovement.startPosition = transform.position;
                enemyMovement.Patrol(); // Quay lại tuần tra
            }
        }
        else
        {
            FindPlayer();
            enemyMovement.Patrol(); // Tiếp tục tuần tra
        }
    }

    private void FindPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null && HasLineOfSight(hit.transform))
        {
            player = hit.transform;
            isChasing = true;
            lastSeenTime = Time.time;
            enemyMovement.StopPatrol(); // Dừng tuần tra khi bắt đầu đuổi
        }
        else
        {
            isChasing = false;
        }
    }

    private bool HasLineOfSight(Transform target)
    {
        if (target == null) return false;

        Vector2 direction = (target.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, obstacleLayer);

        // Chỉ trả về true nếu không có vật cản hoặc vật cản là chính player
        return hit.collider == null || hit.collider.transform == target;
    }

    private void HandleChaseBehavior()
    {
        //if (!isChasing) return;
        if (player == null || isAttacking) return; // dừng di chuyển nếu đang tấn công

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Giữ nguyên velocity.y
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        // Chỉ áp dụng lực theo trục x, giữ nguyên trục y
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (Mathf.Sign(direction.x) != Mathf.Sign(transform.localScale.x))
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
