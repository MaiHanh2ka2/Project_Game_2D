using UnityEngine;

public class Enemy_Radish : Enemy
{

    private RaycastHit2D groundBelowDetected;
    private bool groundAboveDetected;

    [Header("Radish specifics")]
    [SerializeField] private float ceillingDistance;
    [SerializeField] private float groundDistance;

    [SerializeField] private float aggroTime;
                     private float aggroTimeCounter;

    private bool aggresive;
    [SerializeField] private float flyForce;

    protected override void Start()
    {
        base.Start();
    }


    void Update()
    {

        aggroTimeCounter -= Time.deltaTime;

        if (aggroTimeCounter < 0 && !groundAboveDetected)
        {
            rb.gravityScale = 1;
            aggresive = false;
        }

        if (!aggresive)
        {
            if (groundBelowDetected && !groundAboveDetected)
            {
                rb.velocity = new Vector2(0, flyForce);
            }
        }
        else
        {
            if (groundBelowDetected.distance <= 1.25f)
                WalkAround();
        }


        CollisionChecks();

        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("aggresive", aggresive);

    }

    public override void Damage()
    {
        if (!aggresive)
        {
            aggroTimeCounter = aggroTime;
            rb.gravityScale = 12;
            aggresive = true;
        }
        else
            base.Damage();
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        groundAboveDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingDistance, whatIsGround);
        groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
    }

}
