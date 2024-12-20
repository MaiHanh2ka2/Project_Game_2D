using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private InGame_UI inGame_UI;

    private void Start()
    {
        inGame_UI = GameObject.Find("Canvas").GetComponent<InGame_UI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GetComponent<Animator>().SetTrigger("activate");
            AudioManager.instance.PlaySFX(2);
            //PlayerManager.instance.KillPlayer();

            inGame_UI.OnLevelFinished();
            GameManager.instance.CalculateHighScore(); // tinh toan ra diem da nhan duoc hien tai

            GameManager.instance.SaveBestTime();
            Destroy(collision.gameObject);

            GameManager.instance.SaveCollectionFruits();
            GameManager.instance.SaveLevelInfo();
        }
    }
}
