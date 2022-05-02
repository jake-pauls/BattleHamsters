using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {
    [HideInInspector]
    public int pid;
    
    [Header("Nut Management")]
    public int NutCount;
    [SerializeField]
    private GameObject NutPrefab;

    public float PlayerSpeed = 6.0f;

    [SerializeField]
    private float _playerDefaultSpeed = 2.0f;
    [SerializeField]
    private float _jumpHeight = 1.0f;
    [SerializeField]
    private float _gravityValue = -9.81f;
    [SerializeField]
    private float _rotationSpeed = 1000f;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollAcceleration;
    [SerializeField] private float _torqueMultiplier;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Transform _cameraTransform;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _interactAction;
    private InputAction _dropNutAction;
    private Vector2 _moveInput;
    private Vector3 _movementInput => new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
    private GameObject _ball;
    private bool _onBall;

    PlayerModelManager playerModelManager;

    private void Start() {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _cameraTransform = Camera.main.transform;
        _moveAction = _playerInput.actions["Movement"];
        _jumpAction = _playerInput.actions["Jump"];
        _interactAction = _playerInput.actions["Interact"]; 
        _dropNutAction = _playerInput.actions["DropNut"];
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
        _controller.Move(move * Time.deltaTime * PlayerSpeed);

        // Changes the height position of the player
        if (_jumpAction.triggered && _groundedPlayer)
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -8.0f * _gravityValue);

        if (_interactAction.triggered && this.transform.GetChild(0).gameObject.activeSelf) {
            playerModelManager.HideObject(false, this.transform.position);
        }
        
        // Bring the player back down to the ground
        _playerVelocity.y += -35.0f * Time.deltaTime;

        _controller.Move(_playerVelocity * Time.deltaTime);

        // Rotate in the direction of input
        if (move != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(move);

        // Check if interact action was triggered 
        // if (_interactAction.triggered)

        if (_dropNutAction.triggered && NutCount > 0) {
            PlayerSpeed += NutCount / 2.0f;

            NutCount--;
            Vector3 newPos = transform.position - transform.right;
            Instantiate(NutPrefab, newPos, Quaternion.identity);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.transform.tag == "HamsterBall") {
            _ball = hit.gameObject;
            Physics.IgnoreCollision(_ball.GetComponent<Collider>(), GetComponent<Collider>(), true);
            _ball.transform.SetParent(gameObject.transform);
            gameObject.transform.localPosition = _ball.transform.position;
            gameObject.transform.localRotation = _ball.transform.rotation;

        }

        //{
        //    Rigidbody otherrb = hit.collider.attachedRigidbody;
        //    Destroy(otherrb.gameObject);
        //    playerModelManager.HideObject(true, otherrb.transform.position);
        //}

        //if (hit.transform.tag == "Player" && this.transform.GetChild(0).gameObject.activeSelf) {
        //    Rigidbody otherrb = hit.collider.attachedRigidbody;
        //    Destroy(otherrb.gameObject);
        //}
    }

    public void OnMove(InputValue value) {
        _moveInput = value.Get<Vector2>();
        
    }

    private void FixedUpdate() {
        if(_onBall) {
            Rigidbody rb = _ball.GetComponent<Rigidbody>();
            Vector3 targetAcceleration = _movementInput * _rollSpeed;
            Vector3 currentAcceleration = rb.velocity;
            Vector3 finalAcceleration = (targetAcceleration - currentAcceleration) * _rollAcceleration;
            Vector3 Torque = Vector3.Cross(Vector3.up, finalAcceleration);
            rb.AddTorque(Torque * _torqueMultiplier);
            transform.position = _ball.transform.position;
        } else {

        }
    }


}