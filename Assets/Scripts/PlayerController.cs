﻿using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    //lật player  
    //private SpriteRenderer spriteRenderer;

    private Animator animator;
    private bool isGrounded;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;

    //Nhảy kép
    private bool doubleJump;

    //Dash
    public float dashForce = 20f;
    private bool isDashing = false;
    public float dashTime = 0.1f;
    private bool turnRight;   //để lấy hướng Dash

    //Năng lượng
    public int playermaxEnergy = 100;
    private int currentPlayerEnergy;
    public GameManager energyBar;  //quản lý thanh năng lượng
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();

        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    private void Start()
    {
        currentPlayerEnergy = playermaxEnergy;
        energyBar.UpdateEnergyBar(currentPlayerEnergy, playermaxEnergy);
    }

    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;
        HandleMovement();
        HandleJump();
        UpdateAnimation();
        Dash();

        //Tấn công
        //if (Input.GetKeyDown(KeyCode.J) && canAttack) // Nhấn J để tấn công
        //{

            //Debug.Log($"Bấm tấn công - canAttackAgain: {canAttack}");
            // Reset combo nếu không nhấn trong thời gian cho phép
         //   if (Time.time - lastAttackTime > comboResetTime)
         //   {
        //        comboStep = 0;
        //    }
        //    Attack();
        //}
    }

    //Hàm tấn công
    //void Attack()
    //{
    //    canAttack = false;  // Tắt khả năng tấn công liên tục
    //    lastAttackTime = Time.time;
     //   comboStep++;

        // Giới hạn combo tối đa (có 3 đòn chém)
    //    if (comboStep > 3) comboStep = 1;

        // Kích hoạt animation tương ứng
        //animator.SetTrigger("Attack" + comboStep);

        
    //    animator.SetTrigger("Attack");
     //   animator.SetInteger("ComboStep", comboStep); // Điều chỉnh Animator

     //   Debug.Log($"Thực hiện chém {comboStep}");

        // Kiểm tra va chạm với kẻ địch
        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //foreach (Collider2D enemy in hitEnemies)
        //{
            //enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        //}
    //}

    // Animation Event gọi hàm này khi animation gần kết thúc
    //public void CanAttack()
    //{
    //    canAttack = true; // Cho phép tấn công tiếp

    //    Debug.Log("CanAttack() được gọi - có thể tấn công tiếp!");

    //}

    // Animation Event gọi hàm này ở frame cuối cùng để reset combo
    // void ResetCombo()
    //{
     //   comboStep = 0; // Reset về trạng thái ban đầu
     //   canAttack = true;

    //    animator.ResetTrigger("Attack");

        //animator.ResetTrigger("Attack1");
        //animator.ResetTrigger("Attack2");
        //animator.ResetTrigger("Attack3"); // Reset trigger để tránh kẹt animation

    //    Debug.Log("ResetCombo() được gọi - Reset về trạng thái ban đầu");

    //}

    //void OnDrawGizmosSelected()
    //{
     //   if (attackPoint == null) return;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    //}

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (moveInput > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);   //spriteRenderer.flipX = false; //lật mới
                turnRight = true;
            }
            else if (moveInput < 0) {
                transform.localScale = new Vector3(-1, 1, 1);  //spriteRenderer.flipX= true;
                turnRight = false;
            }          
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                audioManager.PlayJumpSound();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                doubleJump = true;
            }
            else if (doubleJump)
            {
                audioManager.PlayJumpSound();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce*0.9f);
                doubleJump = false;
            }
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Nhấn K để dash
        {
            currentPlayerEnergy -= 10;
            energyBar.UpdateEnergyBar(currentPlayerEnergy, playermaxEnergy);

            if (currentPlayerEnergy < 0)
            {
                return;
            }
            else
            {
                float dashDirection = turnRight ? 1 : -1;  //hướng Dash
                rb.linearVelocity = new Vector2(dashForce * dashDirection, rb.linearVelocity.y);
                isDashing = true;
                StartCoroutine(StopDash());
            }
        }
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);  //reset lại sau khi dash
        isDashing= false;
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    public void HealEnergy(int amount)
    {
        currentPlayerEnergy += amount;

        energyBar.UpdateEnergyBar(currentPlayerEnergy, playermaxEnergy);

        if(currentPlayerEnergy > playermaxEnergy)
        {
            currentPlayerEnergy = playermaxEnergy;
        }
    }
}
