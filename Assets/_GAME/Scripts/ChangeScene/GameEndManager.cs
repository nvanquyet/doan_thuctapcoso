using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public GameObject winPanel;  
    public GameObject losePanel; 

    public void ShowWinPopup()
    {
        winPanel.SetActive(true);
        losePanel.SetActive(false);
        Time.timeScale = 0; 
    }

    public void ShowLosePopup()
    {
        losePanel.SetActive(true);
        winPanel.SetActive(false);
        Time.timeScale = 0;
    }

    public void ReplayGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1; 
        Application.Quit();
    }
}
