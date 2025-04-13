using UnityEngine;

public class AfterImage : MonoBehaviour
{
    //public float lifeTime = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    Destroy(gameObject, lifeTime); // Tự xóa sau vài giây
    //}

    public float lifeTime = 1f;
    public float fadeSpeed = 0.5f;

    private SpriteRenderer sr;
    private Color startColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;

        // Nếu bạn muốn thêm hiệu ứng glow, có thể gán material ở đây
        // sr.material = yourGlowMaterial;
    }

    void Update()
    {
        // Giảm alpha theo thời gian để fade-out
        float newAlpha = Mathf.Lerp(sr.color.a, 0f, fadeSpeed * Time.deltaTime);
        sr.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

        // Hủy sau khi alpha gần như bằng 0
        if (sr.color.a <= 0.05f)
        {
            Destroy(gameObject);
        }
    }

}
