using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int difficulty;

    [Header("Timer info")]
    public bool startTime;
    public float timer;
    public float baseTime = 60f;
    public int currentScore;

    [Header("Level info")]
    public int levelNumber;
    public static readonly string HIGHSCORE = "Highscore";

    public int[] highScore;
    private void OnEnable()
    {
        LoadHighScore();
    }
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

        float lastTime = PlayerPrefs.GetFloat("Level" + levelNumber + "BestTime",999);

        if (timer < lastTime)
            PlayerPrefs.SetFloat("Level" + levelNumber + "BestTime", timer);

        timer = 0;
    }

    public void CalculateHighScore()
    {
        int score = 0; 
        int fruit = PlayerManager.instance.fruits; 

        int multiple = Mathf.RoundToInt(Mathf.Max(0, baseTime - timer)); //Tinh toan multiple cua level

        if (multiple > 0)
        {
            score = fruit * multiple; 
        }
        else
        {
            score = fruit * 2; 
        }

        currentScore += score; // gan diem da tinh duoc vao bien currentScore
    }
    public void SaveHighScore()
    {
        string json = JsonUtility.ToJson(new Wrapper(highScore));
        PlayerPrefs.SetString("HighScore", json);
        PlayerPrefs.Save();
    }


    public void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            string json = PlayerPrefs.GetString("HighScore");
            highScore = JsonUtility.FromJson<Wrapper>(json).array;
        }
        else
        {
            highScore = new int[] { 0, 0, 0 };    
        }
    }
    [System.Serializable]
    private class Wrapper
    {
        public int[] array;
        public Wrapper(int[] array)
        {
            this.array = array;
        }
    }
    public void SaveHighScore(int score)
    {
        currentScore = 0;
        for (int i = 0; i < highScore.Length; i++)
        {
            if (score > highScore[i])
            {
                // Dịch chuyển các phần tử phía sau để tạo chỗ trống cho score
                for (int j = highScore.Length - 1; j > i; j--)
                {
                    highScore[j] = highScore[j - 1];
                }

                // Thay thế phần tử tại vị trí i bằng score
                highScore[i] = score;
                break; // Chỉ cần thay thế một lần, nên thoát vòng lặp
            }
        }
        SaveHighScore();
    }
    public static List<int> GetListIntListFromString(string input)
    {
        string[] stringValues = input.Split(':'); // Phân tách chuỗi theo dấu ':'
        List<int> result = new List<int>(); // Khởi tạo danh sách kết quả
        foreach (string s in stringValues)
        {
            try
            {
                // Cố gắng chuyển đổi mỗi chuỗi thành số nguyên và thêm vào danh sách kết quả
                int val = int.Parse(s);
                result.Add(val);
            }
            catch (Exception)
            {
                // Nếu chuyển đổi thất bại, bỏ qua giá trị đó (ngoại lệ được bắt nhưng không xử lý)
            }
        }
        return result; // Trả về danh sách các số nguyên
    }

    // Chuyển đổi một List<int> thành một chuỗi phân tách bằng dấu hai chấm
    public static string GetStringFromListInt(List<int> input)
    {
        return string.Join(":", input); // Nối danh sách thành một chuỗi với dấu ':' làm ký tự phân tách
    }


    // Thuộc tính để lấy và đặt điểm cao trong PlayerPrefs
    public static string Highscore
    {
        get => PlayerPrefs.GetString(HIGHSCORE, "0"); // Lấy điểm cao dưới dạng chuỗi, mặc định là "0"
        set => PlayerPrefs.SetString(HIGHSCORE, value); // Đặt điểm cao trong PlayerPrefs
    }

    // Kiểm tra nếu điểm cao đã cho tồn tại trong danh sách điểm cao đã lưu
    public static bool IsHighscore(int highScore)
    {
        return GetListIntListFromString(Highscore).Contains(highScore); // Chuyển chuỗi thành List<int> và kiểm tra
    }

    // Thêm một điểm cao mới vào danh sách, sắp xếp nó, và giữ lại chỉ 3 điểm cao nhất
    public static void AddNewHighscore(int highscoreID)
    {
        List<int> highscore = GetListIntListFromString(Highscore); // Lấy danh sách điểm cao hiện tại

        // Nếu điểm số chưa có, thêm nó vào danh sách
        if (!highscore.Contains(highscoreID))
        {
            highscore.Add(highscoreID);
        }

        // Sắp xếp điểm theo thứ tự giảm dần (cao hơn trước)
        highscore.Sort((a, b) => b.CompareTo(a));

        // Giữ lại chỉ 3 điểm cao nhất, loại bỏ những điểm thấp hơn
        if (highscore.Count > 3)
        {
            highscore.RemoveAt(highscore.Count - 1);  // Loại bỏ điểm thấp nhất (cuối danh sách)
        }

        // Lưu danh sách điểm cao được cập nhật dưới dạng chuỗi trong PlayerPrefs
        Highscore = GetStringFromListInt(highscore);
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

    private void OnApplicationQuit() 
    {
        AddNewHighscore(currentScore); // save high score
    }
}
