﻿using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Tấn công
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 20;
    public LayerMask enemyLayers;

    private int comboStep = 0;  // Đếm số lần nhấn
    private float lastAttackTime;
    public float comboResetTime = 1.0f;  // Thời gian reset combo
    private bool canAttack = true;


    //Máu player
    private GameManager gameManager;
    public GameManager hPBar; //quuản lý thanh máu
    public int PlayermaxHealth = 200;
    private int currentPlayerHealth;
    

    // Knockback
    //public float knockbackForce = 5f; // Lực knockback
    public float knockbackDuration = 0.2f; // Thời gian knockback
    private bool isKnockback = false; // Kiểm tra xem player có đang bị knockback không
    private Rigidbody2D rb;


    // Update is called once per frame

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của player
    }

    void Start()
    {
        currentPlayerHealth = PlayermaxHealth;
        hPBar.UpdateHPBar(currentPlayerHealth, PlayermaxHealth);
    }

    void Update()
    {
        if (isKnockback) return; // Nếu đang bị knockback (Hurt), không di chuyển

        //Tấn công
        if (Input.GetKeyDown(KeyCode.J) && canAttack) // Nhấn J để tấn công
        {
            //Debug.Log($"Bấm tấn công - canAttackAgain: {canAttack}");
            // Reset combo nếu không nhấn trong thời gian cho phép
            if (Time.time - lastAttackTime > comboResetTime)
            {
                comboStep = 0;
            }
            Attack();
        }
    }

    //Hàm tấn công
    void Attack()
    {
        canAttack = false;  // Tắt khả năng tấn công liên tục
        lastAttackTime = Time.time;
        comboStep++;

        // Giới hạn combo tối đa (có 3 đòn chém)
        if (comboStep > 3) comboStep = 1;

        // Kích hoạt animation tương ứng
        animator.SetTrigger("Attack");
        animator.SetInteger("ComboStep", comboStep); // Điều chỉnh Animator

        Debug.Log($"Thực hiện chém {comboStep}");
    }

    // Hàm này sẽ được gọi bởi Animation Event để kích hoạt hitbox //tấn công đúng tầm
    public void EnableHitbox()
    {
        // Kiểm tra va chạm với kẻ địch
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().EnemyTakeDamage(attackDamage);
        }
    }

    // Animation Event gọi hàm này khi animation gần kết thúc
    public void CanAttack()
    {
        canAttack = true; // Cho phép tấn công tiếp
        Debug.Log("CanAttack() được gọi - có thể tấn công tiếp!");
    }

    // Animation Event gọi hàm này ở frame cuối cùng để reset combo
    public void ResetCombo()
    {
        comboStep = 0; // Reset về trạng thái ban đầu
        canAttack = true;

        animator.ResetTrigger("Attack");

        //animator.ResetTrigger("Attack1");
        //animator.ResetTrigger("Attack2");
        //animator.ResetTrigger("Attack3"); // Reset trigger để tránh kẹt animation

        Debug.Log("ResetCombo() được gọi - Reset về trạng thái ban đầu");
    }

    void OnDrawGizmosSelected()
    {
       if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void PlayerTakeDamage(int damage /*, Transform enemyTransform*/)
    {
        currentPlayerHealth -= damage;
        hPBar.UpdateHPBar(currentPlayerHealth, PlayermaxHealth); // cập nhật lại thanh máu

        //animator.SetTrigger("Hit");

        if (currentPlayerHealth <= 0)
        {
            gameManager.GameOver();
        }
        else
        {
            ApplyKnockback(/*enemyTransform*/); // Áp dụng knockback
        }
    }

    // Áp dụng knockback cho player (giống với enemy)
    private void ApplyKnockback( /*Transform enemyTransform*/ )
    {
        isKnockback = true;
        
        // Kích hoạt animation knockback
        animator.SetTrigger("KnockBackTrigger");
        animator.SetBool("IsKnockBack", true); // Đặt IsKnockback thành true

        // Tính toán hướng knockback
        //Vector2 direction = (transform.position - enemyTransform.position).normalized;
        //direction.y = 0; // Loại bỏ thành phần Y để player không bay lên

        // Áp dụng lực knockback
        //rb.linearVelocity = Vector2.zero; // Reset vận tốc trước khi áp dụng lực
        //rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Bắt đầu coroutine để kết thúc knockback sau một khoảng thời gian
        StartCoroutine(EndKnockback());
    }

    private System.Collections.IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration); // Đợi trong khoảng thời gian knockback
        //rb.linearVelocity = Vector2.zero; // Dừng lại sau khi knockback kết thúc

        // Kết thúc knockback và quay lại trạng thái bình thường
        isKnockback = false; // Kết thúc knockback
        animator.SetBool("IsKnockBack", false);
    }

    //thêm máu khi nhặt healthpotion
    public void Heal(int amount)
    {
        currentPlayerHealth += amount;

        //gameManager.UpdateHPBar(currentPlayerHealth, PlayermaxHealth);  // cập nhật lại thanh máu
        hPBar.UpdateHPBar(currentPlayerHealth, PlayermaxHealth);

        if (currentPlayerHealth > PlayermaxHealth)
        {
            currentPlayerHealth = PlayermaxHealth; // Giới hạn máu tối đa
        }
        Debug.Log("Player healed! Current Health: " + currentPlayerHealth);
    }
}
