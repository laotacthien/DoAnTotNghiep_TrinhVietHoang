using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private PlayerAttack playerAttack;
    private EnemyAttack2 enemyAttack;
    private PlayerController playerController;

    [SerializeField] private int healAmount = 20; // Lượng máu/năng lượng hồi phục

    public Item coinItem, healthPotionItem, energyPotionItem;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        playerAttack = FindAnyObjectByType<PlayerAttack>();
        enemyAttack = FindAnyObjectByType<EnemyAttack2>();
        playerController = FindAnyObjectByType<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            //Xóa coin
            Destroy(collision.gameObject);
            //Thêm vào Inventory
            InventoryManager.Instance.Add(coinItem);

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
            playerAttack.PlayerTakeDamage(enemyAttack.damage);

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
            InventoryManager.Instance.Add(healthPotionItem);
        }
        else if (collision.CompareTag("EnergyPotion"))
        {
            audioManager.PlayCoinSound();
            playerController.HealEnergy(healAmount);

            Destroy(collision.gameObject);
            InventoryManager.Instance.Add(energyPotionItem);
        }
    }
}
