using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class NextLevel : MonoBehaviour
{
    public AudioSource completeLevelAudio;

    // Called after player reaches the end of a level, starts next level
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 < 3)
            {
                completeLevelAudio.Play();

                yield return new WaitForSeconds(completeLevelAudio.clip.length);
                Debug.Log(completeLevelAudio.clip.length);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                
            }
        }
    }
}
