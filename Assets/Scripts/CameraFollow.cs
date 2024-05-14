using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {

        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.fixedDeltaTime * moveSpeed);
        }
    }
}
