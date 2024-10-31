using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Highscore_UI : MonoBehaviour
{
    public TMP_Text highScoreValueTxt;
    public Text[] highscoreTexts;

    private void OnEnable()
    {
        SetupScore(); //Setup diem khi object duoc bat len
        UpdateHighScore();
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
        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            if (i < highscores.Count)
            {
                // Nếu có điểm cho vị trí này, cập nhật text
                highscoreTexts[i].text = highscores[i].ToString();
            }
            else
            {
                // Nếu có ít điểm hơn số lượng thành phần Text, hiển thị "No Record!"
                highscoreTexts[i].text = "No Record!";
            }
        }
    }

    public void SetupScore()
    {
        highScoreValueTxt.text = "Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

}
