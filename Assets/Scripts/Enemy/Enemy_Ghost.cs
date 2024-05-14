using UnityEngine;

public class Enemy_Ghost : Enemy
{
    [Header("Ghost specific")]
    [SerializeField] private float activeTime;
    private float activeTimeCounter = 4;


    private SpriteRenderer sr;

    [SerializeField] private float[] xOffset;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        aggresive = true;
        invincible = true;
    }



    private void Update()
    {
        if (player == null)
        {
            anim.SetTrigger("disappear");
            return;
        }

        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if (activeTimeCounter > 0)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && aggresive)
        {
            anim.SetTrigger("desappear");
            aggresive = false;
            idleTimeCounter = idleTime;
        }

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && !aggresive)
        {
            ChossePosition();
            anim.SetTrigger("appear");
            aggresive = true;
            activeTimeCounter = activeTime;
        }

        FlipController();
    }

    private void FlipController()
    {
        if (player == null)
            return;

        if (facingDirection == -1 && transform.position.x < player.transform.position.x)
            Flip();
        else if (facingDirection == 1 && transform.position.x > player.transform.position.x)
            Flip();
    }

    private void ChossePosition()
    {
        float _xOffset = xOffset[Random.Range(0, xOffset.Length)];
        float _yOffset = Random.Range(-7, 7);
        transform.position = new Vector2(player.transform.position.x + _xOffset, player.transform.position.y + 7 + _yOffset);
    }


    public void Desappear() => sr.enabled = false;
    public void Appear() => sr.enabled = true;
    

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (aggresive)
            base.OnTriggerEnter2D(collision);
    }
}
