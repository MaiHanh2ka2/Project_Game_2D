using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : Trap
{
    public bool isWoking;
    private Animator anim;

    public float repeatRate;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if(transform.parent == null )
            InvokeRepeating("FireSwitch", 0, repeatRate);
    }

    private void Update()
    {
        anim.SetBool("isWorking", isWoking);
    }

    public void FireSwitch()
    {
        isWoking = !isWoking;
    }

    public void FireSwitchAfter(float seconds)
    {
        CancelInvoke();
        isWoking = false;
        Invoke("FireSwitch", seconds);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWoking)
            base.OnTriggerEnter2D(collision);
        
    }
}
