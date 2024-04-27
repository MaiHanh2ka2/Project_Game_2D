using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spiked_Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 pushDirection;


    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        rb.AddForce(pushDirection, ForceMode2D.Impulse);
    }

 
}
