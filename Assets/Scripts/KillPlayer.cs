using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class KillPlayer : MonoBehaviour
{
    //public int Respawn;
    public AudioSource deathSound;
    public GameObject playerObject;
    // private GameMaster gm;
    public GameObject exclamationPoint;
    private GameObject EPinstance; 
    public PlayerMovement playerDeathSpot;

    [SerializeField] float EPDelay = 0.3f; 


    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            deathSound.Play();
            yield return new WaitForSeconds(EPDelay);
            Destroy(playerObject);
            EPinstance = Instantiate(exclamationPoint, playerDeathSpot.playerDeathLoc, Quaternion.identity);
            EPinstance.SetActive(true);
            yield return new WaitForSeconds(deathSound.clip.length);
            DestroyImmediate(EPinstance, true);
            // exclamationPoint.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // exclamationPoint.SetActive(true);
        }
    }

}
