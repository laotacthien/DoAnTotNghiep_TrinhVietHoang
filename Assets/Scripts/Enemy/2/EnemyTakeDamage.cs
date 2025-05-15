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
    public bool isKnockback = false;
    private float knockbackTimer;

    private Rigidbody2D rb;
    private Animator animator;
    private EnemyAI enemyAI;
    private EnemyMovement enemyMovement;
    private PlayerAttack playerAttack;

    public GameObject floatingTextPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        enemyMovement = GetComponent<EnemyMovement>();
        playerAttack = FindAnyObjectByType<PlayerAttack>();

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

        // Hiển thị popup damage (floating text)
        if (floatingTextPrefab)
        {
            //ShowFloatingText();
            CreateFloatingText();
        }

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
        //enemyAI.enabled = false;
        enemyAI.isKnockback = isKnockback;
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
        //enemyAI.enabled = true;
        enemyAI.isKnockback = isKnockback;

        // Reset animation nếu cần
        if (animator != null)
            animator.ResetTrigger("KnockBackTrigger");

        enemyAI.isAttacking = false; // Cho phép enemy di chuyển trở lại
    }
    //void ShowFloatingText()
    //{
    //    //var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
    //    var go = Instantiate(floatingTextPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity); // không có parent  (để đảm bảo text luôn hướng đúng chiều)
    //    //go.GetComponent<TextMesh>().text = currentEnemyHealth.ToString();
    //    if(playerAttack.comboStep == 1)
    //    {
    //        go.GetComponent<TextMesh>().text = playerAttack.attackDamage.ToString();
    //    }
    //    else if(playerAttack.comboStep == 2)
    //    {
    //        go.GetComponent<TextMesh>().text = (playerAttack.attackDamage + 10).ToString();
    //    }
    //    else go.GetComponent<TextMesh>().text = (playerAttack.attackDamage + 20).ToString();

    //    // Đảm bảo luôn hiển thị phía trước các tilemap hoặc foreground
    //    var meshRenderer = go.GetComponent<MeshRenderer>();
    //    meshRenderer.sortingLayerName = "UI";       
    //    meshRenderer.sortingOrder = 10;             // số càng cao càng nằm phía trên
    //}
    void CreateFloatingText()
    {
        // Lấy từ object pool
        GameObject go = FloatingTextPool.Instance.GetFromPool();

        // tắt popup trước khi đặt vị trí
        go.SetActive(false);

        // Đặt vị trí gốc (trước khi áp dụng offset)
        go.transform.position = transform.position;
        go.transform.rotation = Quaternion.identity; // reset xoay nếu cần

        // bật popup để kích hoạt OnEnable()
        go.SetActive(true);

        // hiển thị sát thương
        int damage = playerAttack.attackDamage;
        if (playerAttack.comboStep == 2)
            damage += 10;
        else if (playerAttack.comboStep == 3)
            damage += 20;

        // Gán damage vào text
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.text = "- " + damage.ToString();

        // Luôn hiển thị phía trên các tile
        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "UI";
        meshRenderer.sortingOrder = 10;
    }

    void Die()
    {
        //Destroy(gameObject); // Xóa kẻ địch khi chết
        animator.SetTrigger("Die"); // Kích hoạt animation Die

        enemyHealthBar.gameObject.SetActive(false);
        Destroy(gameObject, 0.3f); // Xóa Enemy sau 1 giây (đủ thời gian chạy animation)
    }
}
