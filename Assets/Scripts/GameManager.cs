using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text countdownTxt;
    public State state;
    public enum State
    {
        Starting,
        Ingame,
        Gameover
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Starting;
        StartCoroutine("Countdown");
    }

    private IEnumerator Countdown()
    {
        countdownTxt.gameObject.SetActive(true);
        countdownTxt.text = "3";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "2";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "1";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "GO!";
        yield return new WaitForSeconds(1);
        countdownTxt.gameObject.SetActive(false);
        state = State.Ingame;

        GameObject.FindAnyObjectByType<CherryController>().GameStart();
        GameObject.FindAnyObjectByType<AudioController>().GhostAliveMusic();
        GameObject.FindAnyObjectByType<TimerManager>().StartTimer();
        GameObject.FindAnyObjectByType<GhostManager>().GameStart();

    }

    public void GameOver()
    {
        state = State.Gameover;
        StartCoroutine("GameOverSequence");
    }

    IEnumerator GameOverSequence()
    {
        int currentScore = GameObject.FindAnyObjectByType<ScoreManager>().currentScore;
        int time = GameObject.FindAnyObjectByType<TimerManager>().time;

        if (currentScore >= PlayerPrefs.GetInt("score"))
        {
            if (currentScore == PlayerPrefs.GetInt("score"))
            {
                if (time < PlayerPrefs.GetInt("time"))
                {
                    PlayerPrefs.SetInt("time", time);
                }
            } else
            {
                PlayerPrefs.SetInt("time", time);
            }
            PlayerPrefs.SetInt("score", currentScore);

            PlayerPrefs.Save();
        }
        

        countdownTxt.text = "GAMEOVER";
        countdownTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

}
