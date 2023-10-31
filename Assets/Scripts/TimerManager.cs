using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public int time = 0;
    public TMP_Text timerText;

    public void StartTimer()
    {
        StartCoroutine("UpdateTimer");
    }

    IEnumerator UpdateTimer()
    {
        yield return new WaitForSeconds(0.01f);
        time++;
        int mins = Mathf.FloorToInt(time / 6000);
        int seconds = Mathf.FloorToInt((time / 100) % 60);
        int milliseconds = Mathf.FloorToInt(time % 100);
        string minString;
        string secString;
        string milString;
        if (mins < 10)
        {
            minString = "0" + mins;
        } else
        {
            minString = mins + "";
        }

        if (seconds < 10)
        {
            secString = "0" + seconds;
        }
        else
        {
            secString = seconds + "";
        }

        if (milliseconds < 10)
        {
            milString = "0" + milliseconds;
        }
        else
        {
            milString = milliseconds + "";
        }
        

        timerText.text = minString + ":" + secString + ":" + milString;
        if (GameObject.FindAnyObjectByType<GameManager>().state == GameManager.State.Ingame)
        {
            StartCoroutine("UpdateTimer");
        }
        
    }

}
