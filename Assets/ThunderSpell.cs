using UnityEngine;

public class ThunderSpell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float lifetime = 3f;
    private float timer;
    public int damage = 5;

    public float damageRadius = 0.5f; // bán kính AOE
    public Transform attackPoint;
    

    private Transform player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
            ThunderPool.Instance.ReturnToPool(gameObject);
        }

    }
    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        collision.GetComponent<PlayerAttack>().PlayerTakeDamage(damage);
    //        //playerAttack.PlayerTakeDamage(damage);
    //        //ArrowPool.Instance.ReturnToPool(gameObject);
    //    }

    //}
    // Gọi từ Animation Event
    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, damageRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerAttack playerAttack = hit.GetComponent<PlayerAttack>();
                if (playerAttack != null)
                {
                    playerAttack.PlayerTakeDamage(damage);
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, damageRadius);
    }
}
