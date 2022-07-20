using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class NextLevel : MonoBehaviour
{
    public AudioSource completeLevelAudio;
    public SceneLoader sceneLoader;
    private int nextSceneIndex;
    // public GameObject crossFadeObject;

    // Called after player reaches the end of a level, starts next level
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (other.CompareTag("Player"))
        {
            if (nextSceneIndex <= sceneLoader.numLevels)
            {
                // audio
                completeLevelAudio.Play();
                yield return new WaitForSeconds(completeLevelAudio.clip.length / 2);

                // start scene change
                // StartCoroutine(sceneLoader.LoadScene(nextSceneIndex, 2f, crossFadeObject));
                StartCoroutine(sceneLoader.LoadScene(nextSceneIndex, 0.8f));
            }
        }
    }
}
