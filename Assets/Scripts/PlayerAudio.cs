using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource step;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("playStep");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator playStep()
    {
        step.Play();
        yield return new WaitForSeconds(0.45f);
        StartCoroutine("playStep");
    }
}
