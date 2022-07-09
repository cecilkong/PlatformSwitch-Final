using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SwitchMap : MonoBehaviour
{
    [SerializeField] private GameObject redMap;
    [SerializeField] private GameObject blueMap;
    private bool blue;
    public CameraShake cameraShake;
    public float shakeMagnitude = .05f;
    public float shakeDuration = .1f;

    public AudioSource switchAudio;
    
    // Start is called before the first frame update
    void Awake()
    {
        redMap.SetActive(false);
        blueMap.SetActive(true);
        blue = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("q"))
        {
            switchAudio.Play();
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));

            if (blue)
            {
                blueMap.SetActive(false);
                redMap.SetActive(true);
                blue = false;
            }
            else
            {
                blueMap.SetActive(true);
                redMap.SetActive(false);
                blue = true;
            }
        }
    }
}
