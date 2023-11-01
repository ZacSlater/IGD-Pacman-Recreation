using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GhostManager : MonoBehaviour
{
    public GameObject[] ghosts;
    AudioController audioController;
    public TMP_Text ghostTimerTxt;
    public GhostController.State globalState = GhostController.State.Walking;
    // Start is called before the first frame update
    void Start()
    {
        audioController = GameObject.FindAnyObjectByType<AudioController>();
    }

    public void GameStart()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostController>().Move();
        }
    }

    public void powerPelletEaten()
    {
        StopCoroutine("GhostTimer");
        StartCoroutine("GhostTimer");
        StopCoroutine("ScaredGhosts");
        StartCoroutine("ScaredGhosts");
    }

    IEnumerator ScaredGhosts()
    {
        audioController.PowerUpMusic();
        globalState = GhostController.State.Scared;
        foreach (GameObject ghost in ghosts)
        {
            Animator anim = ghost.GetComponent<Animator>();
            anim.SetBool("scared", true);
            ghost.GetComponent<GhostController>().state = globalState;
        }

        yield return new WaitForSeconds(7);
        globalState = GhostController.State.Recovering;

        foreach (GameObject ghost in ghosts)
        {
            Animator anim = ghost.GetComponent<Animator>();
            anim.SetBool("recovering", true);
            if (ghost.GetComponent<GhostController>().state != GhostController.State.Dead)
            {
                ghost.GetComponent<GhostController>().state = globalState;
            }
        }

        yield return new WaitForSeconds(3);
        globalState = GhostController.State.Walking;

        foreach (GameObject ghost in ghosts)
        {
            Animator anim = ghost.GetComponent<Animator>();
            anim.SetBool("scared", false);
            anim.SetBool("recovering", false);
            if (ghost.GetComponent<GhostController>().state != GhostController.State.Dead)
            {
                ghost.GetComponent<GhostController>().state = globalState;
            }
        }
        audioController.GhostAliveMusic();
    }

    public bool AllGhostsAlive()
    {
        foreach (GameObject ghost in ghosts)
        {
            if (ghost.GetComponent<GhostController>().state == GhostController.State.Dead)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator GhostTimer()
    {
        int timer = 10;
        ghostTimerTxt.gameObject.SetActive(true);

        while (timer > 0)
        {
            ghostTimerTxt.text = timer + "";
            yield return new WaitForSeconds(1);
            timer--;
        }
        ghostTimerTxt.gameObject.SetActive(false);
    }

}