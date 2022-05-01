using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaterInputHandler : MonoBehaviour
{
    [HideInInspector] public int pid;

    public float _playerSpeed = 2.0f;

    [SerializeField] private float _playerDefaultSpeed = 2.0f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private float _rotationSpeed = 1000f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Transform _cameraTransform;

    PlayerModelManager playerModelManager;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        _cameraTransform = Camera.main.transform;
        playerModelManager = transform.GetComponent<PlayerModelManager>();
    }

    private void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;
    }

    void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log("dasdsa");
    }
    // void OnJump(InputAction.CallbackContext context);
    // void OnInteract(InputAction.CallbackContext context);    
}
