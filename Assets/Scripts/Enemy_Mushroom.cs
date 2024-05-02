using UnityEngine;

public class Enemy_Mushroom : Enemy
{

    
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        WalkAround();

        CollisionChecks();

        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
