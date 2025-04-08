using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventSystemManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /*void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //EventSystem[] systems = FindObjectsOfType<EventSystem>();
        EventSystem[] systems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (systems.Length > 1)
        {
            for (int i = 1; i < systems.Length; i++)
            {
                Destroy(systems[i].gameObject);
            }
        }
    }*/

    private static EventSystemManager instance;

    void Awake()
    {
        var existingEventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (existingEventSystems.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
