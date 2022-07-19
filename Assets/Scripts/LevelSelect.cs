using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    
    [SerializeField] int selectedLevel;

    public AudioSource completeLevelAudio;

    public void LoadSelectedLevel()
    {
        SceneManager.LoadScene(selectedLevel);

    }

    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                completeLevelAudio.Play();

                yield return new WaitForSeconds(completeLevelAudio.clip.length);
                Debug.Log(completeLevelAudio.clip.length);
                if (gameObject.tag == "Levels") {
                    SceneManager.LoadScene("LevelSelect");
                }
                else {
                    SceneManager.LoadScene(selectedLevel);
                }
        }
    }
}