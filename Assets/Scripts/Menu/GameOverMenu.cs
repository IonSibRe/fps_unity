using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    
}
