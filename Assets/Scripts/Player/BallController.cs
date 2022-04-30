using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody _ballRigid;
    [SerializeField] private float torqueSpeed = 360f;
    
    private void Awake()
    {
        _ballRigid = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector2 input)
    {
        _ballRigid.AddTorque(new Vector3(input.x, 0, input.y) * (Time.deltaTime * torqueSpeed));
    }
}
