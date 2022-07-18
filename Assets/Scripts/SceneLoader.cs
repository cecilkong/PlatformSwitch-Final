using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator crossFade;
    public int numLevels;
    public void Start()
    {
        numLevels = 6;
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
