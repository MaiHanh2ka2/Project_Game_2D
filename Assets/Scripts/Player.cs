using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Particles")]
    [SerializeField] private ParticleSystem dustFX;
    private float dustFxTimer;

    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float doubleJumpForce;
    public Vector2 wallJumpDirection;

    private float defaultJumpForce;

    private bool canDoubleJump = true;
    private bool canMove;

    private float movingInput;
    private bool canBeControlled;

    private bool readyToLand;

    [SerializeField] private float bufferJumpTime;
    private float bufferJumpCounter;

    [SerializeField] private float cayoteJumpTime;
    private float cayoteJumpCounter;
    private bool canHaveCayoteJump;

    private float defaultGravityScale;
    [Header("Knockback info")]
    [SerializeField] private Vector2 knockBackDirection;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackProtectionTime;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;
    private bool isGrounded;
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;



    private bool facingRight = true;
    private int facingDirection = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<Collider2D>().enabled = true;
        SetAnimationLayer();

        defaultJumpForce = jumpForce;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControllers();

        if (isKnocked)
            return;

        FlipController();
        CollisionChecks();
        InputChecks();
        CheckForEnemy();

        bufferJumpCounter -= Time.deltaTime;
        cayoteJumpCounter -= Time.deltaTime;

        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;

            if (bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }

            canHaveCayoteJump = true;

            if (readyToLand)
            {
                dustFX.Play();
                readyToLand = false;
            }
        }
        else
        {
            if(!readyToLand)
                readyToLand = true;

            if (canHaveCayoteJump)
            {
                canHaveCayoteJump = false;
                cayoteJumpCounter = cayoteJumpTime;
            }
        }




        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }


        Move();



    }

    private void CheckForEnemy()
    {
        Collider2D[] hitedColliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius);

        foreach (var enemy in hitedColliders)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {

                Enemy newEnemy = enemy.GetComponent<Enemy>();

                if (newEnemy.invincible)
                    return;

                if (rb.velocity.y < 0)
                {
                    AudioManager.instance.PlaySFX(1);
                    newEnemy.Damage();
                    Debug.Log("111");
                    anim.SetBool("flipping", true);
                    Jump();
                }
            }
        }
    }

    private void StopFlippingAnimation()
    {
        anim.SetBool("flipping", false);
    }

    private void SetAnimationLayer()
    {
        int skinIndex = PlayerManager.instance.chosenSkinId;

        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(skinIndex, 1);
    }
    private void AnimationControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetBool("canBeControlled", canBeControlled);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void InputChecks()
    {
        if(!canBeControlled) 
            return;

        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxis("Vertical") < 0)
            canWallSlide = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();

        }
    }

    public void ReturnControl()
    {
        rb.gravityScale = defaultGravityScale;
        canBeControlled = true;
    }
    private void JumpButton()
    {

        if (!isGrounded)
            bufferJumpCounter = bufferJumpTime;


        if (isWallSliding)
        {
            WallJump();
            canDoubleJump = true;
        }

        else if (isGrounded || cayoteJumpCounter > 0)
        {
            Jump();
        }

        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;
        }

        canWallSlide = false;
    }

    public void Knockback(Transform damageTransform)
    {
        AudioManager.instance.PlaySFX(1);

        if (!canBeKnocked)
            return;

        if (GameManager.instance.difficulty > 1)
            PlayerManager.instance.OnTakingDamage();

        PlayerManager.instance.ScreenShake(-facingDirection);
        isKnocked = true;
        canBeKnocked = false;


        #region Define horizontal direction for knockback
        int hDirection = 0;  //h: horizontal

        if (transform.position.x > damageTransform.position.x)
            hDirection = 1;
        else if (transform.position.x < damageTransform.position.x)
            hDirection = -1;
        #endregion

        rb.velocity = new Vector2(knockBackDirection.x * hDirection, knockBackDirection.y);

        Invoke("CancelKnockback", knockbackTime);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }

    private void CancelKnockback()
    {
        isKnocked = false;
    }

    private void AllowKnockback()
    {
        canBeKnocked = true;
    }
    private void Move()
    {
        if (canMove)
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }

    public void Push(float pushForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, pushForce);
    }

    private void WallJump()
    {
        AudioManager.instance.PlaySFX(1);

        canMove = false;
        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);

        dustFX.Play();
    }
    private void Jump()
    {
        AudioManager.instance.PlaySFX(1);

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        if(isGrounded)
            dustFX.Play();
    }

    private void FlipController()
    {
        dustFxTimer -= Time.deltaTime;

        if (facingRight && rb.velocity.x < 0)
            Flip();
        else if (!facingRight && rb.velocity.x > 0)
            Flip();
    }

    public void Flip()
    {
        if(dustFxTimer < 0)
        {
            dustFX.Play();
            dustFxTimer = .7f;
        }
        

        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsWall);

        if (isWallDetected && rb.velocity.y < 0)
            canWallSlide = true;

        if (!isWallDetected)
        {
            isWallSliding = false;
            canWallSlide = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
    }



}
