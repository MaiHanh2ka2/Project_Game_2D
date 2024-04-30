using UnityEngine;

public class Enemy_Rino : Enemy
{

    [Header("Rino spesific")]
    [SerializeField] private float agroSpeed;
    [SerializeField] private float shockTime;
                     private float shockTimeCounter;

    [SerializeField] private LayerMask whatToIgnore;
    private RaycastHit2D playerDetection;
    private bool aggresive;


    protected override void Start()
    {
        base.Start();
        invincible = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 25, ~whatToIgnore);
        if (playerDetection.collider.GetComponent<Player>() != null)
            aggresive = true;

        if (!aggresive)
        {
            WalkAround();
        }

        else
        {
            rb.velocity = new Vector2(agroSpeed * facingDirection, rb.velocity.y);

            if (wallDetected && invincible)
            {
                invincible = false;
                shockTimeCounter = shockTime;
            }


            if (shockTimeCounter <= 0 && !invincible)
            {
                invincible = true;
                Flip();
                aggresive = false;
            }

            shockTimeCounter -= Time.deltaTime;
        }


        CollisionChecks();
        AnimatorController();
    }

    private void AnimatorController()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDirection, wallCheck.position.y));
    }
}
