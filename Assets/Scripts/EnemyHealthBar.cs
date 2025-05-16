using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    //thanh máu enemy
    [Header("Health Bar")]
    public Slider enemyHPBar;
    public Color low;
    public Color high;
    public Vector3 offSet;

    [Header("Boss Settings")]
    public Slider bossHPBar; // Slider dành riêng cho Boss
    

    //cập nhật thanh máu enemy
    public void UpdateenemyHPBar(float currentHealth, float maxHealth)
    {
        if (transform.parent.CompareTag("Boss"))
        {
            bossHPBar.gameObject.SetActive(currentHealth < maxHealth);
            //bossHPBar.gameObject.SetActive(true);
            bossHPBar.maxValue = maxHealth;
            bossHPBar.value = currentHealth;
            bossHPBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, bossHPBar.normalizedValue);

            if(currentHealth <= 0)
            {
                //Destroy(bossHPBar.gameObject);
                bossHPBar.gameObject.SetActive(false);
            }
        }
        else
        {
            //chỉ xuất hiện khi bị mất máu
            enemyHPBar.gameObject.SetActive(currentHealth < maxHealth);

            enemyHPBar.value = currentHealth; //giá trị thanh máu bằng máu hiện tại
            enemyHPBar.maxValue = maxHealth; //giá trị thanh máu tối đa bằng máu tối đa

            //Màu của thanh máu bằng mức độ tổn thương
            enemyHPBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, enemyHPBar.normalizedValue);
        }
        
    }

    

    // Update is called once per frame
    void Update()
    {
        //if (transform.parent.CompareTag("Boss")) return;
        //thanh mmáu di chuyển theo enemy (con theo cha)
        enemyHPBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offSet);
    }
}
