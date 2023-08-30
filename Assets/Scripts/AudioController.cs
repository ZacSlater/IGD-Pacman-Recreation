using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource levelLoad;
    public AudioSource ghostAlive;
    public AudioSource ghostDead;
    public AudioSource powerUpActive;
    // Start is called before the first frame update
    void Start()
    {
        levelLoad.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelLoad.isPlaying && !ghostAlive.isPlaying)
        {
            ghostAlive.Play();
        }
    }
}
