using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HamsterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = .5f;
    
    private Rigidbody _hamsterRigid;

    private void Awake()
    {
        _hamsterRigid = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector2 input)
    {
        _hamsterRigid.MovePosition(new Vector3(input.x, 0, input.y) * (Time.deltaTime * moveSpeed));
    }
}
