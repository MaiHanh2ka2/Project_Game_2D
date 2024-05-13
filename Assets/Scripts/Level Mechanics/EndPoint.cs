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

            inGame_UI.OnLevelFinished();

            Destroy(collision.gameObject);

            GameManager.instance.SaveBestTime();
            GameManager.instance.SaveCollectionFruits();
            GameManager.instance.SaveLevelInfo();
           
        }
    }
}