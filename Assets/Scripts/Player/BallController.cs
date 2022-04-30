using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody _ballRigid;
    private float torqueSpeedPerSec = 360f;
    [SerializeField] private float maxTorque = 200f;
    
    private void Awake()
    {
        _ballRigid = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector2 input)
    {
        _ballRigid.AddTorque(new Vector3(input.x, 0, input.y) * (Time.fixedDeltaTime * torqueSpeedPerSec) * maxTorque);
    }
}
