using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Header("Plant specifics")]

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool facingRight;
    protected override void Start()
    {
        base.Start();
        if (facingRight)
            Flip();
    }

    private void Update()
    {
        CollisionChecks();
        bool playerDetected = false;
        idleTimeCounter -= Time.deltaTime;
        if (playerDetection)
        {
            if (playerDetection.collider.GetComponent<Player>() != null)
            {
                playerDetected = true;
            }
        }

        if (idleTimeCounter < 0 && playerDetected)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("attack");
        }
    }


    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);

        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
        Destroy(newBullet, 3f);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
