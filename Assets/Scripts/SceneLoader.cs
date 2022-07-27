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
    [SerializeField] private float levelStartDelay = 2f;

    // public NextLevel crossFadeObj;
    // private GameObject CFO; 

    // void Awake()
    // {
    //     CFO = crossFadeObj.crossFadeObject.SetActive(false);
    // }

    public void Start()
    {
        levelText.text = "level " + SceneManager.GetActiveScene().buildIndex.ToString();
        numLevels = 12;
    }

    // public IEnumerator LoadScene(int sceneIndex, float levelStartDelay, GameObject crossFadeObject)
    public IEnumerator LoadScene(int sceneIndex, float levelStartDelay)
    {
        if (sceneIndex <= numLevels)
        {
            crossFade.SetTrigger("Start");
            yield return new WaitForSeconds(levelStartDelay);
            SceneManager.LoadScene(sceneIndex);
            // crossFadeObject.SetActive(false);
        }
    }
}
