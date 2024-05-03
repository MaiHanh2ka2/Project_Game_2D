using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat specific")]
    [SerializeField] private Transform[] idlePoint;

    private Vector2 destination;
    private bool canBeAggresive = true;
    private bool playerDetected;
    private Transform player;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;

    float defaultSpeed;


    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        defaultSpeed = speed;
        destination = idlePoint[0].position;
        transform.position = idlePoint[0].position;
    }



    void Update()
    {
        anim.SetBool("canBeAggresive", canBeAggresive);
        anim.SetFloat("speed", speed);

        idleTimeCounter -= Time.deltaTime;
        if (idleTimeCounter > 0)
            return;

        playerDetected = Physics2D.OverlapCircle(transform.position, whatIsPlayer);

        if (playerDetected && !aggresive && canBeAggresive)
        {
            aggresive = true;
            canBeAggresive = false;
            destination = player.transform.position;
        }

        if (aggresive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                aggresive = false;

                int i = Random.Range(0, idlePoint.Length);
                destination = idlePoint[i].position;
                speed = speed * .5f;
            }
        }

        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                if (!canBeAggresive)
                    idleTimeCounter = idleTime;

                canBeAggresive = true;
                speed = defaultSpeed;
            }

        }
        FlipController();
    }

    public override void Damage()
    {
        base.Damage();
        idleTimeCounter = 5;
    }
    private void FlipController()
    {
        if (facingDirection == -1 && transform.position.x < destination.x)
            Flip();
        else if (facingDirection == 1 && transform.position.x > destination.x)
            Flip();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

}

