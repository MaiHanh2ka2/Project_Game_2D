using UnityEngine;

public class Enemy_BlueBird : Enemy
{

    private bool ceillingDetected;

    [Header("Blue bird specifics")]
    [SerializeField] private float ceillingDistance;
    [SerializeField] private float groundDistance;


    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
    private float flyForce;

    private bool canFly = true;

    public override void Damage()
    {
        canFly = false;
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        base.Damage();
    }

    protected override void Start()
    {
        base.Start();
        flyForce = flyUpForce;
    }

    private void Update()
    {
        CollisionChecks();

        if (ceillingDetected)
            flyForce = flyDownForce;
        else if (groundDetected)
            flyForce = flyUpForce;

        if (wallDetected)
            Flip();


    }

    [SerializeField] private Transform movePoint;
    [SerializeField] private float xMultiplier;
    [SerializeField] private float yMultiplier;
     
    public void FlyUpEvent()
    {
        if (canFly)
            rb.velocity = new Vector2(speed * facingDirection, flyForce);

    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
    }
}
