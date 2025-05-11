using UnityEngine;

public class FloatingText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 offset = new Vector3(0f, 1f, 0f);
    public Vector3 randomizeIntensity = new Vector3(0.5f, 0f, 0f);

    private Vector3 startPosition;

    void OnEnable()
    {
        // Gán vị trí hiển thị
        startPosition = transform.position + offset;
        startPosition += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x), 0, 0);
        transform.position = startPosition;
    }
    
    void Disable()
    {
        FloatingTextPool.Instance.ReturnToPool(gameObject);
    }
    void OnDisable()
    {
        CancelInvoke();
    }

}
