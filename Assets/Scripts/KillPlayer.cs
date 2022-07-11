using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class KillPlayer : MonoBehaviour
{
    public int Respawn;
    public AudioSource deathSound;
    //public GameObject playerObject;
    // private GameMaster gm;


    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            deathSound.Play();
            //Destroy(playerObject);
            yield return new WaitForSeconds(deathSound.clip.length);
            SceneManager.LoadScene(Respawn);
        }
    }

}
