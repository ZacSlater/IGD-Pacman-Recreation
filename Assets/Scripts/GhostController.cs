using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public State state;
    Animator anim;
    public enum State
    {
        Walking,
        Scared,
        Recovering,
        Dead
    }

    void Start()
    {
        state = State.Walking;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillGhost()
    {
        state = State.Dead;
        anim.SetBool("scared", false);
        anim.SetBool("recovering", false);
        anim.SetBool("dead", true);
        GameObject.FindAnyObjectByType<ScoreManager>().increaseScore(300);
        GameObject.FindAnyObjectByType<AudioController>().GhostDeadMusic();
        StartCoroutine("DeadTime");
    }

    public IEnumerator DeadTime()
    {
        yield return new WaitForSeconds(5);
        GameObject.FindAnyObjectByType<AudioController>().GhostAliveMusic();
        anim.SetBool("dead", false);
        state = State.Walking;
    }    

}
