using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SkinSelection_UI : MonoBehaviour
{

    private int skin_Id;

    [SerializeField] private bool[] skinPurchased;
    [SerializeField] private int[] priceForSkin;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI bankText;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject selectButton, selectToNewGame;
    [SerializeField] private Animator anim;


    private void Start()
    {
        PlayerPrefs.SetInt("TotalFruitsCollected", 1000);
    }

    private void SetupSkinInfo()
    {
        skinPurchased[0] = true;

        for(int i = 1; i < skinPurchased.Length; i++)
        {
            bool skinUnLocked = PlayerPrefs.GetInt("SkinPurchased" + i) == 1;
            if (skinUnLocked)
            {
                skinPurchased[i] = true;
            }
        }

        bankText.text = PlayerPrefs.GetInt("TotalFruitsCollected").ToString();

        buyButton.SetActive(!skinPurchased[skin_Id]);
        selectButton.SetActive(skinPurchased[skin_Id]);
        selectToNewGame.SetActive(skinPurchased[skin_Id]);


        if (!skinPurchased[skin_Id])
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Price: " + priceForSkin[skin_Id];

        anim.SetInteger("skinId", skin_Id);
    }

    public bool EnoughMoney()
    {
        int totalFruits = PlayerPrefs.GetInt("TotalFruitsCollected");

        if (totalFruits > priceForSkin[skin_Id])
        {
            totalFruits = totalFruits - priceForSkin[skin_Id];

            PlayerPrefs.SetInt("TotalFruitsCollected", totalFruits);

            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        SetupSkinInfo();
    }
    private void OnDisable()
    {
        selectButton.SetActive(false);
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
        if (EnoughMoney())
        {
            PlayerPrefs.SetInt("SkinPurchased" + skin_Id, 1);
            SetupSkinInfo();
        }
        else
            Debug.Log("NotEnoughMoney");
    }
    public void Select()
    {
        PlayerManager.instance.chosenSkinId = skin_Id;
    }
    public void SwitchSelectButton(GameObject newButton)
    {
        selectButton = newButton;
    }
}
