using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("level1");
    }

    public void QuitGame() { 
        Application.Quit();
    }

    //gọi Scene trong bảng chonj level (bằng tên scene)
    public void Openlevel(int levelID)
    {
        string levelName = "level" + levelID;
        SceneManager.LoadScene(levelName);
    }

    //khoá các cấp độ level khi chưa vượt qua
    public Button[] buttons;
    private void Awake()
    {
        //PlayerPrefs.SetInt("UnlockedLevel", 1); // Reset về 1 khi vào game
        //PlayerPrefs.Save(); // Lưu thay đổi


        //int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        //for(int i =0; i<buttons.Length; i++)
        //{
        //    buttons[i].interactable = false;
        //}
        //for(int i =0; i< Mathf.Min(unlockedLevel, buttons.Length); i++)  //Giới hạn unlockedLevel không vượt quá số lượng buttons.Length, tránh lỗi truy cập ngoài phạm vi.
        //{
        //    buttons[i].interactable = true;
        //}
    }

    //thêm cả pause menu
    [SerializeReference] GameObject pauseMenu;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        //trở lại màn đang chơi
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    
}
