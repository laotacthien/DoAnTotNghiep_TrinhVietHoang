using UnityEngine;

public class LightBlade : MonoBehaviour
{
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
            LightBladePool.Instance.ReturnToPool(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAttack.PlayerTakeDamage(damage);
            ArrowPool.Instance.ReturnToPool(gameObject);
        }

    }

}
