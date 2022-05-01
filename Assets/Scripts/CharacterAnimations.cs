using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;
    private HamsterController hamControl;

    void Start()
    {
        _rb = transform.parent.GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        hamControl = transform.parent.GetComponent<HamsterController>();
    }

    void Update()
    {
        float velocity = hamControl.BallMode ? hamControl.BallRef.GetComponent<Rigidbody>().angularVelocity.magnitude : _rb.velocity.magnitude;
        _animator.SetFloat("Speed", velocity) ;

        // Debug.Log(_rb.velocity.magnitude);
    }
}
