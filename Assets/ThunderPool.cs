using System.Collections.Generic;
using UnityEngine;

public class ThunderPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ThunderPool Instance;

    public GameObject thunderSpellPrefab;
    public int poolSize = 1;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(thunderSpellPrefab);
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
            // Nếu hết pool thì tạo mới
            GameObject obj = Instantiate(thunderSpellPrefab);
            return obj;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
