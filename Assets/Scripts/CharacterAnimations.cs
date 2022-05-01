using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator 
        _animator;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        bool ballMode = GetComponent<BallRotation>()._ballMode;
        float velocity = ballMode ? GetComponent<BallRotation>()._ball.GetComponent<Rigidbody>().angularVelocity.magnitude : _rb.velocity.magnitude;
        _animator.SetFloat("Speed", velocity) ;

        Debug.Log(_rb.velocity.magnitude);
    }
}
