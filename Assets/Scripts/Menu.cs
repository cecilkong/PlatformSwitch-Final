using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pauseScreen;
    // Start is called before the first frame update
    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f; 
    }
    
    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
