using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour
{
    [Header("Shooting (jush archer)")]
    public float attackRange2 = 5f;
    public float attackCooldown2 = 2f;

    private EnemyAI enemyAI;
    private Animator animator;
    private Shooting shooting;
    private EnemyAttack2 enemyAttack2;

    private bool isShooting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
        shooting = GetComponent<Shooting>();
        enemyAttack2 = GetComponent<EnemyAttack2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAI.Player == null || isShooting) return;

        float distance = Vector2.Distance(transform.position, enemyAI.Player.position);
      
        if (distance <= attackRange2 && !isShooting)
        {
            StartCoroutine(PerformAttack2());
        }
    }
    private IEnumerator PerformAttack2()
    {
        isShooting = true;
        enemyAI.isAttacking = true; // thông báo cho EnemyAI dừng di chuyển

        // Kiểm tra lại line of sight trước khi bắn
        //if (!enemyAI.HasLineOfSight(enemyAI.Player))
        //{
        //    isShooting = false;
        //    enemyAI.isAttacking = false;
        //    yield break;
        //}
        animator.SetTrigger("Shoot");

        yield return new WaitForSeconds(attackCooldown2);
        //enemyAI.isAttacking = false;
        isShooting = false;

    }
    public void EnableShoot()
    {
        shooting.Shoot();
    }
    public void EnableShootBlade()
    {
        shooting.ShootBlade();
    }
    public void EnableThunderSpell()
    {
        shooting.ShootThunder();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange2);
    }
}
