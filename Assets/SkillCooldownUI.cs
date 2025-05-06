using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image cooldownImage; // Gán ảnh fill
    public float cooldownTime = 5f;

    private float cooldownTimer = 0f;
    private bool isCoolingDown = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownImage.fillAmount = cooldownTimer / cooldownTime;

            if (cooldownTimer <= 0f)
            {
                isCoolingDown = false;
                cooldownImage.fillAmount = 0f;
            }
        }
    }
    public void TriggerCooldown()
    {
        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        cooldownImage.fillAmount = 1f;
    }

    public bool IsReady()
    {
        return !isCoolingDown;
    }
}
