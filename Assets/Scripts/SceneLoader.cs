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
    public GameObject crossFadeObject;

    public void Start()
    {
        levelText.text = "level " + SceneManager.GetActiveScene().buildIndex.ToString();
        numLevels = 12;
    }

    public IEnumerator LoadScene(int sceneIndex, float levelStartDelay, GameObject crossFadeObject)
    {
        if (sceneIndex <= numLevels)
        {
            crossFade.SetTrigger("Start");
            yield return new WaitForSeconds(levelStartDelay);
            SceneManager.LoadScene(sceneIndex);
            yield return new WaitForSeconds(1f);
            //crossFadeObject.gameObject.SetActive(false);
            crossFadeObject.SetActive(false);
        }
    }
}
