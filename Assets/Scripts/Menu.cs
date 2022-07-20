using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Manages all of the menu navigation and buttons
    
    public GameObject pauseScreen;
    private bool isPaused;

    void Awake()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    
    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;
    }

    // Restarts current level
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    
    public void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelect");
        Time.timeScale = 1f;
    }
    
    // Called after the player reaches the goal of each level
    // public void LoadNextLevel()
    // {
    //     if (SceneManager.GetActiveScene().buildIndex + 1 < 3)
    //     {
    //         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //     }
    // }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}
