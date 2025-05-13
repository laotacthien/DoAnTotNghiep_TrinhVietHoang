using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private PlayerRespawn playerRespawn;
    private Animator animator;
    private Collider2D coll;

    public Transform respawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerRespawn = FindAnyObjectByType<PlayerRespawn>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRespawn.UpdateCheckPoint(respawnPoint.position);
            animator.enabled = true;
            coll.enabled = false;
        }
    }
}
