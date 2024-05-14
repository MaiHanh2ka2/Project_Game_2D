using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int difficulty;

    [Header("Timer info")]
    public bool startTime;
    public float timer;

    [Header("Level info")]
    public int levelNumber;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {

        if (difficulty == 0)
            difficulty = PlayerPrefs.GetInt("GameDifficulty");
    }

    private void Update()
    {
        if (startTime)
            timer += Time.deltaTime;
    }

    public void SaveGameDifficulty()
    {
        PlayerPrefs.SetInt("GameDifficulty", difficulty);
    }

    public void SaveBestTime()
    {
        startTime = false;

        float lastTime = PlayerPrefs.GetFloat("Level" + levelNumber + "BestTime", 999);

        if (timer < lastTime)
            PlayerPrefs.SetFloat("Level" + levelNumber + "BestTime", timer);

        timer = 0;
    }

    public void SaveCollectionFruits()
    {
        int totalFruits = PlayerPrefs.GetInt("TotalFruitsCollected", 0);

        int newTotalFruits = totalFruits + PlayerManager.instance.fruits;

        PlayerPrefs.SetInt("TotalFruitsCollected", newTotalFruits);
        PlayerPrefs.SetInt("Level" + levelNumber + "FruitsCollected", PlayerManager.instance.fruits);

        PlayerManager.instance.fruits = 0;
    }

    public void SaveLevelInfo()
    {
        int nextLevelNumber = levelNumber + 1;
        PlayerPrefs.SetInt("Level" + nextLevelNumber + "Unlocked", 1);
    }
}
