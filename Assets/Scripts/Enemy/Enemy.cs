using UnityEngine;

public class Enemy : Danger
{
    protected Animator anim;
    protected Rigidbody2D rb;

    protected int facingDirection = -1;

    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatToIgnore;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float playerDistance = 100f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    protected RaycastHit2D playerDetection;


    protected Transform player;
    protected bool wallDetected;
    protected bool groundDetected;

    public bool invincible;

    [Header("Move info")]
    [SerializeField] protected float speed;
    [SerializeField] protected float idleTime = 2;
    [SerializeField]protected float idleTimeCounter;


    protected bool canMove = true;
    public bool aggresive;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (PlayerManager.instance.currentPlayer != null)
            player = PlayerManager.instance.currentPlayer.transform;

        if (groundCheck == null)
            groundCheck = transform;
        if (wallCheck == null)
            wallCheck = transform;
    }

    protected virtual void WalkAround()
    {
        if (idleTimeCounter <= 0 && canMove)
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, 0);   // Vector2.zero 

        idleTimeCounter -= Time.deltaTime;


        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
    }
    public virtual void Damage()
    {
        //Deal damage from player to enemy
        if (!invincible)
            canMove = false;
        anim.SetTrigger("gotHit");
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }



    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void CollisionChecks()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        // checks if the player is within the detection range of the enemy
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerDistance, whatIsPlayer);
    }

    protected virtual void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        if (wallCheck != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        }
    }
}
