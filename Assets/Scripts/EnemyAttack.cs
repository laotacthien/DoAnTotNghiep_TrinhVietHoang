using UnityEngine;
using System.Collections;
public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // Phạm vi tấn công
    public float attackCooldown = 1.5f; // Thời gian giữa mỗi lần tấn công
    public Transform player; // Tham chiếu đến Player
    public LayerMask playerLayer; // Lớp để nhận diện Player
    public int enemyDamage = 10; // damage

    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator animator;
    //private bool isHitboxActive = false; // Kiểm tra xem hitbox có đang hoạt động không

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer(); // Kiểm tra Player trong phạm vi tấn công   
    }

    void DetectPlayer()
    {
        Collider2D playerInRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        if (playerInRange != null && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack"); // Gọi animation chém kiếm

        yield return new WaitForSeconds(attackCooldown); // Chờ hết thời gian cooldown

        isAttacking = false;
    }

    // Hàm này sẽ được gọi bởi Animation Event để kích hoạt hitbox  -  Gây sát thương cho player bên trong
    public void EnableHitbox()
    {
        //float distance = Vector2.Distance(transform.position, player.transform.position);
        //if (distance > attackRange) return; // Không gây sát thương nếu Player đã rời phạm vi
        //player.GetComponent<PlayerAttack>().PlayerTakeDamage(enemyDamage);

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hitPlayer == null) return; // Không gây sát thương nếu Player đã rời phạm vi

        PlayerAttack playerAttack = hitPlayer.GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.PlayerTakeDamage(enemyDamage);
        }
        //isHitboxActive = true;
        Debug.Log("Hitbox enabled");
    }

    // Hàm này sẽ được gọi bởi Animation Event để tắt hitbox
    public void DisableHitbox()
    {
        //isHitboxActive = false;
        Debug.Log("Hitbox disabled");
    }

    // Xử lý va chạm với Player
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (isHitboxActive && collision.CompareTag("Player"))
    //    {
    //        PlayerAttack player = collision.GetComponent<PlayerAttack>();
    //        if (player != null)
    //        {
    //           player.PlayerTakeDamage(enemyDamage); // Gây sát thương cho Player
    //        }
    //    }
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Vẽ phạm vi tấn công trong Scene
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }
}
