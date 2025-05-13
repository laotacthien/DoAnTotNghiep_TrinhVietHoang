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

    [Header("Shooting (jush archer)")]
    public float attackRange2 = 5f;
    public float attackCooldown2 = 2f;


    private EnemyAI enemyAI;
    private Animator animator;
    private bool isAttacking = false;

    private Shooting shooting;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();

        shooting = GetComponent<Shooting>();
    }

    void Update()
    {
        if (enemyAI.Player == null || isAttacking) return;

        float distance = Vector2.Distance(transform.position, enemyAI.Player.position);
        if (distance <= attackRange * 2)
        {
            StartCoroutine(PerformAttack());
        }
        else if(distance <= attackRange2)
        {
            StartCoroutine(PerformAttack2());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        enemyAI.isAttacking = true; // thông báo cho EnemyAI dừng di chuyển

        animator.SetTrigger("Attack");
        //animator.SetBool("isAttacking", isAttacking);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        //enemyAI.isAttacking = false; // Cho phép di chuyển trở lại
    }
    private IEnumerator PerformAttack2()
    {
        isAttacking = true;
        enemyAI.isAttacking = true; // thông báo cho EnemyAI dừng di chuyển

        // Kiểm tra lại line of sight trước khi bắn
        if (!enemyAI.HasLineOfSight(enemyAI.Player))
        {
            isAttacking = false;
            enemyAI.isAttacking = false;
            yield break;
        }

        animator.SetTrigger("Shoot");
        
        //animator.SetBool("isAttacking", isAttacking);

        //yield return new WaitForSeconds(0.4f); // thời gian delay trước khi bắn
        //shooting.Shoot();

        //enemyAI.isAttacking = false; // Cho phép di chuyển trở lại
        yield return new WaitForSeconds(attackCooldown2);
        isAttacking = false;
        
    }
    public void EnemyShoot()
    {
        shooting.Shoot();
    }
    public void EnemyContinueChase()
    {
        enemyAI.isAttacking = false; // Cho phép enemy di chuyển trở lại
    }

    public void EnableHitbox()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyAI.playerLayer);
        player?.GetComponent<PlayerAttack>()?.PlayerTakeDamage(damage);
        //enemyAI.isAttacking = false; // Cho phép enemy di chuyển trở lại
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange2);
    }
}
