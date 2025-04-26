using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Hệ thống máu Enemy
    [Header("Heal Settings")]
    public int enemymaxHealth = 50;
    private int currentEnemyHealth;
    public EnemyHealthBar enemyHealthBar; //goi toi thanh maus enemy


    [Header("Knockback Settings")]
    public float knockbackDuration = 0.2f;
    public float knockbackForce = 5f;
    private bool isKnockback = false;
    private float knockbackTimer;

    private Rigidbody2D rb;
    private Animator animator;
    private EnemyAI enemyAI;
    private EnemyMovement enemyMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        enemyMovement = GetComponent<EnemyMovement>();

        currentEnemyHealth = enemymaxHealth;
        enemyHealthBar.UpdateenemyHPBar(currentEnemyHealth, enemymaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                EndKnockback();
            }
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        // Giảm máu
        currentEnemyHealth -= damage;
        enemyHealthBar.UpdateenemyHPBar(currentEnemyHealth, enemymaxHealth);

        // Knockback
        animator.SetTrigger("KnockBackTrigger");
        ApplyKnockback(knockbackDirection);

        // Xử lý chết
        if (currentEnemyHealth <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback(Vector2 direction)
    {
        isKnockback = true;
        knockbackTimer = knockbackDuration;

        // Tạm dừng AI và movement
        enemyAI.enabled = false;
        if (enemyMovement != null)
            enemyMovement.StopPatrol();

        // Áp dụng lực knockback
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);

        // Kích hoạt animation knockback nếu có
        if (animator != null)
            animator.SetTrigger("KnockBackTrigger");
    }

    private void EndKnockback()
    {
        isKnockback = false;
        rb.linearVelocity = Vector2.zero;

        // Kích hoạt lại AI
        enemyAI.enabled = true;

        // Reset animation nếu cần
        if (animator != null)
            animator.ResetTrigger("KnockBackTrigger");
    }
    void Die()
    {
        //Destroy(gameObject); // Xóa kẻ địch khi chết
        animator.SetTrigger("Die"); // Kích hoạt animation Die

        enemyHealthBar.gameObject.SetActive(false);
        Destroy(gameObject, 0.3f); // Xóa Enemy sau 1 giây (đủ thời gian chạy animation)
    }
}
