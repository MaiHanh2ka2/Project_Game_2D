using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_UI : MonoBehaviour
{
    public void SwitchMenuTo(GameObject wiMenu)
    {
        for (int i = 0; i <transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        wiMenu.SetActive(true);
    }
}
