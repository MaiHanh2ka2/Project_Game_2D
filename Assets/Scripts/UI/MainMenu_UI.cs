using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu_UI : MonoBehaviour
{

    [SerializeField] private GameObject continueButton;
    [SerializeField] private VolumeController_UI[] volumeController;

    private void Start()
    {
        bool showButton = PlayerPrefs.GetInt("Level" + 2 + "Unlocked") == 1;
        continueButton.SetActive(showButton);


        for (int i = 0; i < volumeController.Length; i++)
        {
            volumeController[i].GetComponent<VolumeController_UI>().SetupVolumeSlider();
        }

        AudioManager.instance.PlayBGM(0);
    }
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i <transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        AudioManager.instance.PlaySFX(1);
        uiMenu.SetActive(true);
    }

    public void SetGameDifficulty(int i) => GameManager.instance.difficulty = i;
}
