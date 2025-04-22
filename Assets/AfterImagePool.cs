using UnityEngine;
using System.Collections.Generic;

public class AfterImagePool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static AfterImagePool Instance;

    public GameObject afterImagePrefab;
    public int poolSize = 15;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(afterImagePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // Nếu hết pool thì tạo mới (hoặc có thể từ chối)
            GameObject obj = Instantiate(afterImagePrefab);
            return obj;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
