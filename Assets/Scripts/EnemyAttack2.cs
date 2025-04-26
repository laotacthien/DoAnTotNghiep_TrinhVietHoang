using UnityEngine;
using System.Collections;

public class EnemyAttack2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    private EnemyAI enemyAI;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (enemyAI.Player == null || isAttacking) return;

        float distance = Vector2.Distance(transform.position, enemyAI.Player.position);
        if (distance <= attackRange)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        //animator.SetBool("isAttacking", isAttacking);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // Called from Animation Event
    public void EnableHitbox()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyAI.playerLayer);
        player?.GetComponent<PlayerAttack>()?.PlayerTakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
