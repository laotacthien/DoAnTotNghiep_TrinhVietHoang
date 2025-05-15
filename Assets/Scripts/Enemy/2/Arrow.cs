using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 3f;
    private float timer;
    public int damage = 5;
    private PlayerAttack playerAttack;

    public LayerMask groundLayer;
    private void Awake()
    {
        playerAttack = FindAnyObjectByType<PlayerAttack>();
    }

    void OnEnable()
    {
        timer = 0f;

        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
        transform.rotation = Quaternion.identity;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ArrowPool.Instance.ReturnToPool(gameObject);
        }


        // Xoay mũi tên theo hướng bay
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAttack.PlayerTakeDamage(damage);
            ArrowPool.Instance.ReturnToPool(gameObject);
        }


        //va chạm với mặt đất
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            var rb = GetComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
