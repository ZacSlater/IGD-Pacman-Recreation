using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource levelLoad;
    public AudioSource ghostAlive;
    public AudioSource ghostDead;
    public AudioSource powerUpActive;
    public AudioSource currentMusic;
    // Start is called before the first frame update
    void Start()
    {
        currentMusic = ghostAlive;
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentMusic.isPlaying && GameObject.FindAnyObjectByType<GameManager>().state == GameManager.State.Ingame)
        {
            currentMusic.Play();
        }
    }

    public void PowerUpMusic()
    {
        levelLoad.Stop();
        currentMusic.Stop();
        currentMusic = powerUpActive;
    }

    public void GhostAliveMusic()
    {
        currentMusic.Stop();
        currentMusic = ghostAlive;
    }

    public void GhostDeadMusic()
    {
        currentMusic.Stop();
        currentMusic = ghostDead;
    }

    public bool IsGhostMusicOn()
    {
        return currentMusic == ghostDead;
    }

}
