using UnityEngine;

public class DontdestroyAudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static DontdestroyAudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //di chuyển đối tượng trình quản lý trò chơi giữa các cảnh
        }
        else //if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    
}
