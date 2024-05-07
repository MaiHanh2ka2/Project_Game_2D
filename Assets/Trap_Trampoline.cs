using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{
    [SerializeField] private float pushForce = 20;
    [SerializeField] private bool canBeUsed = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && canBeUsed)
        {
            canBeUsed = false;
            GetComponent<Animator>().SetTrigger("activate");
            collision.GetComponent<Player>().Push(pushForce);
        }

    }

    private void CanUseAgain() => canBeUsed = true;

}
