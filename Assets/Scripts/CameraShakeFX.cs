using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeFX : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impusle;
    [SerializeField] private Vector3 shakeDirection;
    [SerializeField] private float forceMultiplier;
    public void ScreenShake(int facingDir)
    {
        impusle.m_DefaultVelocity = new Vector3(shakeDirection.x * facingDir, shakeDirection.y) * forceMultiplier;
        impusle.GenerateImpulse();
    }
}
