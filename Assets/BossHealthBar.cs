using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    //thanh máu boss
    [Header("Boss Settings")]
    public Slider bossHPBar; // Slider dành riêng cho Boss
    public Color low;
    public Color high;

    //cập nhật thanh máu boss
    public void UpdatebossHPBar(float currentHealth, float maxHealth)
    {
        bossHPBar.maxValue = maxHealth;
        bossHPBar.value = currentHealth;
        bossHPBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, bossHPBar.normalizedValue);

        if (currentHealth <= 0)
        {
            //Destroy(bossHPBar.gameObject);
            bossHPBar.gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            bossHPBar.gameObject.SetActive(true);
        }
    }
    
}
