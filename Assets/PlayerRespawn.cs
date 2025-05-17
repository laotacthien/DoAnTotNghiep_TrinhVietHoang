using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector2 checkPointPos;

    private PlayerAttack playerAttack;
    private GameManager gameManager;

    private bool isRespawning = false; // chặn hồi sinh nhiều lần

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Start()
    {
        checkPointPos = transform.position;
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos = pos;
    }

    public void Die()
    {
        if (isRespawning) return;

        //nếu còn mạng thì quay lại điểm hồi sinh
        if (gameManager.respawnNumber > 1)
        {
            gameManager.respawnNumber -= 1;
            StartCoroutine(Respawn(0.5f));
        }
        else gameManager.GameOver();
    }
    IEnumerator Respawn(float duration)
    {
        isRespawning = true;
        yield return new WaitForSeconds(duration);
        transform.position = checkPointPos;
        playerAttack.currentPlayerHealth = playerAttack.PlayermaxHealth;
        gameManager.UpdateHPBar(playerAttack.currentPlayerHealth, playerAttack.PlayermaxHealth); // cập nhật lại thanh máu

        //yield return new WaitForSeconds(0.1f);
        //gameManager.respawnNumber -= 1;
        isRespawning = false;
    }
}
