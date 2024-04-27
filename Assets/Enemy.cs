using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public void Damage()
    {
        Debug.Log("I was damaged!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            Player player = collision.collider.GetComponent<Player>();

            player.Knockback(transform);
        }
    }
}
