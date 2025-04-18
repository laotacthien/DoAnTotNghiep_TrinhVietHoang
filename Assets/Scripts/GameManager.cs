﻿using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int score = 0; // tính điểm
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    private bool isGameOver = false;
    private bool isGameWin = false;

    //thanh máu player
    public Image hPBar;

    //cập nhật thanh máu player
    public void UpdateHPBar(float currentHealth, float maxHealth)
    {
        hPBar.fillAmount = currentHealth / maxHealth;
    }
    //thanh năng lượng
    public Image energyBar;

    //cập nhật thanh năng lượng
    public void UpdateEnergyBar(float currentEnergy, float maxEnergy)
    {
        energyBar.fillAmount = currentEnergy / maxEnergy;
    }

    void Start()
    {
        UpdateScore();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int points) //Lượng điểm muốn tăng khi player chạm vào
    {
        if (!isGameOver && !isGameWin)
        {
            score += points;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        if (scoreText == null)
        {
            Debug.LogError("LỖI: scoreText chưa được gán hoặc đã bị xóa!");
            return;
        }
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void GameWin()
    {
        if (gameWinUI == null)
        {
            Debug.LogError("LỖI: gameWinUI chưa được gán hoặc đã bị xóa!");
            return;
        }

        isGameWin = true;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
    }

    public void RestartGame() { 
        isGameOver = false;
        score = 0;
        UpdateScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("level1");
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

}
