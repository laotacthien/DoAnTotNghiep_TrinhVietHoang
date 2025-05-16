using UnityEngine;

public class FireBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float lifetime = 3f;
    private float timer;
    public int damage = 5;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAttack = FindAnyObjectByType<PlayerAttack>();
    }

    void OnEnable()
    {
        timer = 0f;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            FireBallPool.Instance.ReturnToPool(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAttack.PlayerTakeDamage(damage);
            FireBallPool.Instance.ReturnToPool(gameObject);
        }
        if (collision.CompareTag("Ground"))
        {
            FireBallPool.Instance.ReturnToPool(gameObject);
        }
    }
}
