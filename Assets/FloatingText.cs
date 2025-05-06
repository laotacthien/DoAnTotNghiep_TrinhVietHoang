using UnityEngine;

public class FloatingText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float destroyTime = 3f;
    public Vector3 offSet = new Vector3 (0f, 2f, 0f);
    public Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);
    void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.position += offSet;
        transform.position += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x), 0, 0);
    }
    //void Update()
    //{
    //    DirectionText();
    //}
    //void DirectionText()
    //{
    //    if(transform.localScale.x < 0)
    //    {
    //        Vector3 scale = transform.localScale;
    //        scale.x *= -1;
    //        transform.localScale = scale;
    //    }
    //}
}
