using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu_UI : MonoBehaviour
{

    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        bool showButton = PlayerPrefs.GetInt("Level" + 2 + "Unlocked") == 1;
        continueButton.SetActive(showButton);
    }
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i <transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
    }

    public void SetGameDifficulty(int i) => GameManager.instance.difficulty = i;
}
