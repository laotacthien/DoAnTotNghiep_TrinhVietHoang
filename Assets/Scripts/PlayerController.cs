using System.Collections;
//using System.Drawing;
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

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;
    
    //lật player  
    //private SpriteRenderer spriteRenderer;

    private Animator animator;
    public bool isGrounded;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;

    //Nhảy kép
    private bool doubleJump;

    //Dash
    public float dashForce = 20f;
    private bool isDashing = false;
    public float dashTime = 0.2f;
    private bool turnRight;   //để lấy hướng Dash

    public GameObject afterImagePrefab; // gắn trong Inspector
    public float afterImageInterval = 0.03f; // tần suất tạo tàn ảnh
    private float afterImageTimer = 0f;


    //Wallslide và WallJump
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;  //hướng nhảy tường
    private float wallJumpingTime = 0.2f;  //thời gian nhảy tường
    private float wallJumpingCounter;  //bộ đếm nhảy tường
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);  //lực nhảy

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

        // Nếu đang Dash thì tạo tàn ảnh định kỳ
        if (isDashing)
        {
            afterImageTimer -= Time.deltaTime;
            if (afterImageTimer <= 0f)
            {
                CreateAfterImage(new Color(0.5f, 0.8f, 1f, 1f));
                afterImageTimer = afterImageInterval;
            }
        }

        //Nếu đang Wall jump thì tạo tàn ảnh định kỳ
        if (isWallJumping)
        {
            afterImageTimer -= Time.deltaTime;
            if (afterImageTimer <= 0f)
            {
                CreateAfterImage(new Color(1f, 1f, 1f, 0.6f));
                afterImageTimer = 3 * afterImageInterval;
            }
        }

        WallSlide();
        WallJump();

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

        if (!isDashing && !isWallJumping)
        {
            //audioManager.PlayRunSound();
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (moveInput > 0)
            {
                //audioManager.PlayRunSound();
                transform.localScale = new Vector3(1, 1, 1);   //spriteRenderer.flipX = false; //lật mới
                turnRight = true;
            }
            else if (moveInput < 0) 
            {
                //audioManager.PlayRunSound();
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(isGrounded)
            {
                currentPlayerEnergy -= 1;
                energyBar.UpdateEnergyBar(currentPlayerEnergy, playermaxEnergy);

                if (currentPlayerEnergy < 0)
                {
                    return;
                }
                else
                {
                    audioManager.PlayDashSound();
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 2f);
                }
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Nhấn K để dash
        {
            currentPlayerEnergy -= 1;
            energyBar.UpdateEnergyBar(currentPlayerEnergy, playermaxEnergy);

            if (currentPlayerEnergy < 0)
            {
                return;
            }
            else
            {
                audioManager.PlayDashSound();
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

    void CreateAfterImage(Color color)
    {
        Debug.Log("Tạo tàn ảnh!");

        //GameObject clone = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
        //SpriteRenderer cloneSR = clone.GetComponent<SpriteRenderer>();
        //SpriteRenderer playerSR = GetComponent<SpriteRenderer>();

        // Copy sprite và hướng
        //cloneSR.sprite = playerSR.sprite;

        //if (turnRight) cloneSR.flipX = false;  //cloneSR.flipX = playerSR.flipX;
        //else cloneSR.flipX = true;
        //cloneSR.sortingOrder = playerSR.sortingOrder - 1; // để tàn ảnh nằm sau

        // Màu mờ
        //cloneSR.color = new Color(0.5f, 0.8f, 1f, 1f);

        GameObject afterImage = AfterImagePool.Instance.GetFromPool();
        afterImage.transform.position = transform.position;
        afterImage.transform.rotation = transform.rotation;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        afterImage.GetComponent<AfterImage>().SetData(sr.sprite, !turnRight, color);

    }

    private bool IsWalled()  //kiểm tra chạm tường
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if(IsWalled() && !isGrounded && Input.GetAxisRaw("Horizontal") != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0)
        {
            audioManager.PlayWallJumpSound();

            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                turnRight = !turnRight;
                Vector3 localScale = transform.localScale;
                localScale *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("isDashing", isDashing);
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
