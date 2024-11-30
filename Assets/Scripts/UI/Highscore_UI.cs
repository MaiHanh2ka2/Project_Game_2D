using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Highscore_UI : MonoBehaviour
{
    public TMP_Text[] highScoreValueTxt;
    public Text[] highscoreTexts;

    private void OnEnable()
    {
        SetupScore(); //Setup diem khi object duoc bat len
    }

    private List<int> LoadHighScores()
    {
        string highscoreString = GameManager.Highscore;
        List<int> highscoreList = GameManager.GetListIntListFromString(highscoreString);
        return highscoreList;
    }

    public void UpdateHighScore()
    {
        List<int> newHighscores = LoadHighScores();

        UpdateScoreToUI(newHighscores);
    }

    public void SetHighscore(int score)
    {
        GameManager.AddNewHighscore(score);
    }

    private void UpdateScoreToUI(List<int> highscores)
    {
        SetupScore();
    }

    public void SetupScore()
    {
        int[] highScore = GameManager.instance.highScore;
        for (int i = 0; i < highScore.Length; i++)
        {
            highScoreValueTxt[i].text = highScore[i].ToString();
        }  
    }

}
