using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Danger
{
    private Rigidbody2D rb;

    private float xSpeed;
    private float ySpeed;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }

    public void SetupSpeed(float x, float y)
    {
        xSpeed = x;
        ySpeed = y;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.GetComponent<Enemy>() != null)
        //{
        //    collision.GetComponent<Enemy>().Damage();
        //}

        base.OnTriggerEnter2D(collision);
        Destroy(gameObject);
    }
}
