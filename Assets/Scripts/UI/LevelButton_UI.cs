using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton_UI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private TextMeshProUGUI collectedFruits;
    [SerializeField] private TextMeshProUGUI totalFruits;

    public void UpdateTextInfo(int levelNumber)
    {
        levelName.text = "Level " + levelNumber;
        bestTime.text = "Best time: " + PlayerPrefs.GetFloat("Level" + levelNumber + "BestTime", 999).ToString("00") + " sec";
        collectedFruits.text = PlayerPrefs.GetInt("Level" + levelNumber + "FruitsCollected", PlayerManager.instance.fruits).ToString();
        totalFruits.text = "/ " + PlayerPrefs.GetInt("Level" + levelNumber + "TotalFruits");
    }
}
