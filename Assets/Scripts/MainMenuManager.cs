using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text highscoreText;
    public TMP_Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("score") == false)
        {
            PlayerPrefs.SetInt("score", 0);
        }

        if (PlayerPrefs.HasKey("time") == false)
        {
            PlayerPrefs.SetInt("time", 0);
        }

        PlayerPrefs.Save();

        highscoreText.text = PlayerPrefs.GetInt("score") + ""; 
        int time = PlayerPrefs.GetInt("time");

        int mins = Mathf.FloorToInt(time / 6000);
        int seconds = Mathf.FloorToInt((time / 100) % 60);
        int milliseconds = Mathf.FloorToInt(time % 100);
        string minString;
        string secString;
        string milString;
        if (mins < 10)
        {
            minString = "0" + mins;
        }
        else
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


        timeText.text = minString + ":" + secString + ":" + milString;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
