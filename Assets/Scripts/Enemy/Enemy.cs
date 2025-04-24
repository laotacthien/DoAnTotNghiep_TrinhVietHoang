using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Patrol Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f;

    private Vector3 startPos; //Điểm ban đầu của enemy
    private bool movingRight = true;

    //private UnityEngine.Transform player; // liên quan đến knockback
    private UnityEngine.Transform player; //Tham chiếu đến player để tính toán hướng knockback

    //Hệ thống máu Enemy
    [Header("Heal Settings")]
    public int enemymaxHealth = 50;
    private int currentEnemyHealth;
    public EnemyHealthBar enemyHealthBar; //goi toi thanh maus enemy
    


    //damage
    //public int enemyDamage = 10;

    //knockback
    //public float knockbackForce = 2f;
    // Knockback
    public float knockbackForce = 5f; // Lực knockback
    public float knockbackDuration = 0.2f; // Thời gian knockback
    private bool isKnockback = false; // Kiểm tra xem enemy có đang bị knockback không


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // liên quan đến knockback - tìm player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        startPos = transform.position; //vị trí hiện tại của enemy

        currentEnemyHealth = enemymaxHealth;
        enemyHealthBar.UpdateenemyHPBar(currentEnemyHealth, enemymaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();

        
    }

    void Patrol()
    {
        if (isKnockback) return; // Nếu đang bị knockback, không di chuyển

        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip() //Di chuyển lặp lại
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void EnemyTakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
        enemyHealthBar.UpdateenemyHPBar(currentEnemyHealth, enemymaxHealth);

        animator.SetTrigger("KnockBackTrigger");

        if (currentEnemyHealth <= 0)
        {
            Die();
        }
        else
        {
            KnockBack();
        }
    }

    //void KnockBack()
    //{
    //    Vector2 direction = (transform.position - player.position).normalized;
    //    rb.linearVelocity = Vector2.zero; // Reset vận tốc trước khi đẩy
    //    rb.linearVelocity = direction * knockbackForce; // Áp dụng knockback
    //}
    void KnockBack()
    {
        if (player == null) return; // Kiểm tra nếu player không tồn tại

        // Kích hoạt animation knockback
        animator.SetTrigger("KnockBackTrigger");
        animator.SetBool("IsKnockBack", true); // Đặt IsKnockback thành true

        // Tính toán hướng knockback
        Vector2 direction = (transform.position - player.position).normalized;
        direction.y = 0; // Loại bỏ thành phần Y

        // Áp dụng lực knockback
        isKnockback = true;
        rb.linearVelocity = Vector2.zero; // Reset vận tốc trước khi áp dụng lực
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Bắt đầu coroutine để kết thúc knockback sau một khoảng thời gian
        StartCoroutine(EndKnockback());
    }

    private System.Collections.IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration); // Đợi trong khoảng thời gian knockback
        rb.linearVelocity = Vector2.zero; // Dừng lại sau khi knockback kết thúc
        isKnockback = false; // Kết thúc knockback
        animator.SetBool("IsKnockBack", false);
    }

    void Die()
    {
        //Destroy(gameObject); // Xóa kẻ địch khi chết
        animator.SetTrigger("Die"); // Kích hoạt animation Die

        enemyHealthBar.gameObject.SetActive(false);
        Destroy(gameObject, 0.3f); // Xóa Enemy sau 1 giây (đủ thời gian chạy animation)
    }
}
