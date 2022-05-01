using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {
    [HideInInspector]
    public int pid;

    public float _playerSpeed = 2.0f;

    [SerializeField]
    private float _playerDefaultSpeed = 2.0f;
    [SerializeField]
    private float _jumpHeight = 1.0f;
    [SerializeField]
    private float _gravityValue = -9.81f;
    [SerializeField]
    private float _rotationSpeed = 1000f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Transform _cameraTransform;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _interactAction;

    PlayerModelManager playerModelManager;
    [SerializeField] private Transform hamTrans;
    
    private void Start() {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _cameraTransform = Camera.main.transform;
        _moveAction = _playerInput.actions["Movement"];
        _jumpAction = _playerInput.actions["Jump"];
        _interactAction = _playerInput.actions["Interact"];
        playerModelManager = transform.GetComponent<PlayerModelManager>();
    }

    private void Update() {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        Vector2 input = _moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * _cameraTransform.right.normalized + move.z * _cameraTransform.forward.normalized;
        move.y = 0f;
        if (isBallActive)
        {
                        
        }
        else
        {
            _controller.Move(move * Time.deltaTime * _playerSpeed);
        
            // Bring the player back down to the ground
            _playerVelocity.y += -35.0f * Time.deltaTime;

            _controller.Move(_playerVelocity * Time.deltaTime);

            // Rotate in the direction of input
            if (move != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(move);

            // Check if interact action was triggered 
            // if (_interactAction.triggered)   
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if (hit.transform.tag == "HamsterBall")
        //{
        //    //Debug.Log("hit");
        //    Rigidbody otherrb = hit.collider.attachedRigidbody;
        //    //Destroy(otherrb.gameObject);

        //    // mount the ball
        //    isBallActive = true;
        //    hamTrans.gameObject.SetActive(false);
        //    transform.SetParent(hit.transform);
        //    Physics.IgnoreCollision(hit.collider, GetComponent<CharacterController>());
        //    GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //}
    }


    private bool isBallActive;
}