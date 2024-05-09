using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelButton;
    [SerializeField] private Transform levelButtonParent;

    [SerializeField] private bool[] levelOpen;

    private void Start()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (!levelOpen[i])
                return;


            string sceneName = "Level " + i;
            Debug.Log(sceneName);

            GameObject newButton = Instantiate(levelButton, levelButtonParent);
            newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;
        }
    }
    public void LoadLevel(string sceneName) => SceneManager.LoadScene(sceneName);
}