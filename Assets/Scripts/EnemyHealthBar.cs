using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    //thanh máu enemy
    public Slider enemyHPBar;
    public Color low;
    public Color high;
    public Vector3 offSet;

    //cập nhật thanh máu enemy
    public void UpdateenemyHPBar(float currentHealth, float maxHealth)
    {
        //chỉ xuất hiện khi bị mất máu
        enemyHPBar.gameObject.SetActive(currentHealth < maxHealth);

        enemyHPBar.value = currentHealth; //giá trị thanh máu bằng máu hiện tại
        enemyHPBar.maxValue = maxHealth; //giá trị thanh máu tối đa bằng máu tối đa

        //Màu của thanh máu bằng mức độ tổn thương
        enemyHPBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, enemyHPBar.normalizedValue);
    }

    // Update is called once per frame
    void Update()
    {

        //thanh mmáu di chuyển theo enemy (con theo cha)
        enemyHPBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offSet);
    }
}
