using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinSelection_UI : MonoBehaviour
{
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private Animator anim;
    [SerializeField] private int skin_Id;

    [SerializeField] private bool[] skinPurchased;
    [SerializeField] private int[] priceForSkin;


    private void Start()
    {
        skinPurchased[0] = true;
    }


    private void SetupSkinInfo()
    {
        equipButton.SetActive(skinPurchased[skin_Id]);
        buyButton.SetActive(!skinPurchased[skin_Id]);


        if (!skinPurchased[skin_Id])
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Price: " + priceForSkin[skin_Id];

        anim.SetInteger("skinId", skin_Id);
    }
    public void NextSkin()
    {
        skin_Id++;

        if (skin_Id > 3)
            skin_Id = 0;
        SetupSkinInfo();
    }

    public void PreviousSkin() 
    {
        skin_Id--;

        if (skin_Id < 0)
            skin_Id = 3;

        SetupSkinInfo();
    }

    public void Buy()
    {
        skinPurchased[skin_Id] = true;

        SetupSkinInfo();
    }

    public void Select()
    {
        PlayerManager.instance.chosenSkinId = skin_Id;
        Debug.Log("Skin was equip");
    }
}
