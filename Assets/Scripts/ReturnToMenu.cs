using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ReturnToMenu : MonoBehaviour
{
    public AudioSource completeLevelAudio;
    public SceneLoader sceneLoader;

    // Called after player reaches the end of a level, starts next level
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // audio
            completeLevelAudio.Play(); 
            yield return new WaitForSeconds(completeLevelAudio.clip.length / 2);

            // start scene change
            StartCoroutine(sceneLoader.LoadScene(0));
        }
    }
}