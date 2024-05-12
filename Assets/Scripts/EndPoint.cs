using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GetComponent<Animator>().SetTrigger("activate");

            Destroy(collision.gameObject);

            GameManager.instance.SaveBestTime();
            GameManager.instance.SaveCollectionFruits();
            GameManager.instance.SaveLevelInfo();
           
        }
    }
}
