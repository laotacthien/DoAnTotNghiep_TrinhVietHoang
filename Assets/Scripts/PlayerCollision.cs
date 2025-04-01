using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private PlayerAttack playerAttack;
    private EnemyAttack enemyAttack;
    private PlayerController playerController;

    [SerializeField] private int healAmount = 20; // Lượng máu/năng lượng hồi phục
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        playerAttack = FindAnyObjectByType<PlayerAttack>();
        enemyAttack = FindAnyObjectByType<EnemyAttack>();
        playerController = FindAnyObjectByType<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            audioManager.PlayCoinSound();
            gameManager.AddScore(1);
            
        }
        else if (collision.CompareTag("Trap"))
        {
            //gameManager.GameOver();
            playerAttack.PlayerTakeDamage(10);
        }
        else if (collision.CompareTag("Enemy"))
        {
            // nhận sát thương khi player chạm vào enemy
            playerAttack.PlayerTakeDamage(enemyAttack.enemyDamage);

        }
        else if (collision.CompareTag("KeyVictory"))
        {
            gameManager.GameWin();
            Destroy(collision.gameObject);

        }
        else if (collision.CompareTag("HealthPotion"))
        {
            audioManager.PlayCoinSound();
            playerAttack.Heal(healAmount);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("EnergyPotion"))
        {
            audioManager.PlayCoinSound();
            playerController.HealEnergy(healAmount);
            Destroy(collision.gameObject);
        }
    }
}
