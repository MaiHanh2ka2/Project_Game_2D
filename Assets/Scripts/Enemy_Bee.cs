using UnityEngine;

public class Enemy_Bee : Enemy
{
    [Header("Bee specifics")]
    [SerializeField] private Transform[] idlePoint;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float yOffset;
    [SerializeField] private float aggroSpeed;

    private bool playerDetected;
    private int idlePointIndex;
    private Transform player;
    private float defaultSpeed;

    [Header("Bullet specifics")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private float bulletSpeed;

    protected override void Start()
    {
        base.Start();
        defaultSpeed = speed;
        player = GameObject.Find("Player").transform;
    }


    // Update is called once per frame
    void Update()
    {
        bool idle = idleTimeCounter > 0;
        anim.SetBool("idle", idle);

        idleTimeCounter -= Time.deltaTime;

        if (idle)
            return;


        playerDetected = Physics2D.OverlapCircle(playerCheck.position, whatIsPlayer);

        if (playerDetected && !aggresive)
        {
            aggresive = true;
            speed = aggroSpeed;
        }

        if (!aggresive)
        {
            transform.position = Vector2.MoveTowards(transform.position, idlePoint[idlePointIndex].position, speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, idlePoint[idlePointIndex].position) < .1f)
            {
                idlePointIndex++;
                if(idlePointIndex >= idlePoint.Length)
                    idlePointIndex = 0;
            }
        }

        else
        {

            Vector2 newPosition = new Vector2(player.transform.position.x, player.transform.position.y + yOffset);
            transform.position = Vector2.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

            float xDifference = transform.position.x - player.position.x;

            if (Mathf.Abs(xDifference) < .15f)
            {
                anim.SetTrigger("attack");

            }

        }
    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);

        newBullet.GetComponent<Bullet>().SetupSpeed(0, -speed);
        Destroy(newBullet, 3f);

        speed = defaultSpeed;
        idleTimeCounter = idleTime;
        aggresive = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(playerCheck.position, checkRadius);
    }
}
