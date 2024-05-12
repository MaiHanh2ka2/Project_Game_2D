using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{

    private bool gamePaused;

    [Header("Menu gameobjects")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject endLevelUI;

    [Header("TextComponents")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI currentFruitAmount;

    [SerializeField] private TextMeshProUGUI endTimerText;
    [SerializeField] private TextMeshProUGUI endBestTimeText;
    [SerializeField] private TextMeshProUGUI endFruitsText;
    [SerializeField] Button retryBtn, mainMenuBtn;

    private void Start()
    {
        retryBtn.onClick.AddListener(ReLoadCurrentLevel);
        mainMenuBtn.onClick.AddListener(LoadMainMenu);
        GameManager.instance.startTime = true;
        GameManager.instance.timer = 0;
        GameManager.instance.levelNumber = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1;
        SwitchUI(inGameUI);
    }
    void Update()
    {
        UpdateInGameInfo();

        if (Input.GetKeyDown(KeyCode.Escape))
            CheckIfNotPaused();
    }

    private bool CheckIfNotPaused()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0;
            SwitchUI(pauseUI);
            return true;
        }
        else
        {
            gamePaused = true;
            Time.timeScale = 1;
            SwitchUI(inGameUI);
            return false;
        }
    }

    public void OnLevelFinished()
    {
        endFruitsText.text = "fruits: " + PlayerManager.instance.fruits;
        endTimerText.text = "Your time: " + GameManager.instance.timer.ToString("00") + " s";
        endBestTimeText.text = "Best time: " + PlayerPrefs.GetFloat("Level" + GameManager.instance.levelNumber + "BestTime", 999).ToString("00") + " s";

        SwitchUI(endLevelUI);
    }
    private void UpdateInGameInfo()
    {
        timerText.text = "Timer: " + GameManager.instance.timer.ToString("00") + " s";
        currentFruitAmount.text = PlayerManager.instance.fruits.ToString();
    }

    public void SwitchUI(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Debug.Log("Load Main");
        SceneManager.LoadScene("Main Menu");
    }
    public void ReLoadCurrentLevel()
    {
        Debug.Log("Load Current");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
