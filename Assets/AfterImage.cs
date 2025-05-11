using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float fadeSpeed = 0.3f;  //0.5f;

    private SpriteRenderer sr;
    private Color startColor;

    //void Start()
    //{
    //    sr = GetComponent<SpriteRenderer>();
    //    startColor = sr.color;

    //}

    void OnEnable()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        startColor = sr.color;
    }

    void Update()
    {
        // Giảm alpha theo thời gian để fade-out
        //float newAlpha = Mathf.Lerp(sr.color.a, 0f, fadeSpeed * Time.deltaTime);

        // Mờ dần theo thời gian (ổn định hơn cái trên)
        float newAlpha = sr.color.a - fadeSpeed * Time.deltaTime;
        newAlpha = Mathf.Clamp01(newAlpha); // giữ trong [0,1]

        sr.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

        // Hủy sau khi alpha gần như bằng 0
        if (sr.color.a <= 0.05f)
        {
            //Destroy(gameObject);

            AfterImagePool.Instance.ReturnToPool(gameObject);
        }
    }

    public void SetData(Sprite sprite, bool flipX, Color color)
    {
        sr.sprite = sprite;
        sr.flipX = flipX;
        sr.color = color;
        startColor = color;
    }
}
