using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    // this one responsible for giving damage to the player
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();
            var playerManager =  PlayerManager.instance;
            player.Knockback(transform);
            if (playerManager.fruits >= 0)
            {
                playerManager.fruits--;
                if (playerManager.fruits <= 0)
                {
                    playerManager.fruits = 0;
                    playerManager.KillPlayer();
                }
            }
        }
    }
}
