using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    private float count = 0;
    [SerializeField] private float startGameDelay = 4f;
    [SerializeField] private GameObject darkBackground;
    [SerializeField] private GameObject lightBackground;
    [SerializeField] private GameObject darkText;
    [SerializeField] private GameObject lightText;
    

    void Update()
    {
        // Switches the background and text color
        if (Input.GetKeyDown("q") || Input.GetKeyDown(KeyCode.LeftShift))
        {
            count++;
        }
        if (count % 2 == 0)
        {
            lightBackground.SetActive(false);
            darkText.SetActive(false);
            darkBackground.SetActive(true);
            lightText.SetActive(true);
        }
        else
        {
            darkBackground.SetActive(false);
            lightText.SetActive(false);
            lightBackground.SetActive(true);
            darkText.SetActive(true);
        }

        // The game starts in a few seconds after the player switches the
        // color of the background and text twice
        if (count >= 2)
        {
            Invoke("StartGame", startGameDelay);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level01");
    }
}
