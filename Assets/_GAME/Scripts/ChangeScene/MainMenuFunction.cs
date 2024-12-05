using UnityEngine.SceneManagement;
using UnityEngine;
public class MainMenu : MonoBehaviour
{
    public void start()
    {
        SceneManager.LoadScene(2);
    }
    public void exitToMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void exit()
    {
        Application.Quit();
    }
    public void login()
    {
        SceneManager.LoadScene(1);
    }
}
