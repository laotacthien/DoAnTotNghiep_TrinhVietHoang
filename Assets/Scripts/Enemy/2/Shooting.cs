using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform firePoint;
    public float arrowSpeed = 20f;

    public void Shoot()
    {
        GameObject arrow = ArrowPool.Instance.GetFromPool();
        arrow.transform.position = firePoint.position;

        float directionX = transform.localScale.x > 0 ? 1f : -1f;
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        //rb.linearVelocity = new Vector2(directionX * arrowSpeed, 0f);

        rb.linearVelocity = Vector3.zero; //reset vận tốc
        Vector2 shootDir = new Vector2(directionX, 0.2f).normalized; // Bay chếch lên nhẹ
        rb.AddForce(shootDir * arrowSpeed, ForceMode2D.Impulse);


        // Xoay hoặc scale nếu cần
        arrow.transform.localScale = new Vector3(directionX, 1f, 1f);
    }
    public void ShootBlade()
    {
        GameObject lightBlade = LightBladePool.Instance.GetFromPool();
        lightBlade.transform.position = firePoint.position;

        float directionX = transform.localScale.x > 0 ? 1f : -1f;
        Rigidbody2D rb = lightBlade.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(directionX * arrowSpeed, 0f);

        // Xoay hoặc scale nếu cần
        lightBlade.transform.localScale = new Vector3(directionX, 1f, 1f);
    }

}
