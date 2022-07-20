using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator crossFade;
    public int numLevels;
    public TextMeshProUGUI levelText;
    public void Start()
    {
        levelText.text = "level " + SceneManager.GetActiveScene().buildIndex.ToString();
        numLevels = 12;
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        if (sceneIndex <= numLevels)
        {
            crossFade.SetTrigger("Start");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
