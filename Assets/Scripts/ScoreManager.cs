using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public TMP_Text scoreTxt; 
    void Start()
    {
        currentScore = 0;
    }

    public void increaseScore(int increase)
    {
        currentScore += increase;
        scoreTxt.text = "Score: " + currentScore;
    }
}
