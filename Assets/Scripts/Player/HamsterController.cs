using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class HamsterController : MonoBehaviour
{
    private Vector2 _moveInput;
    private Vector3 _movementInput;
    private Rigidbody _rb;
    public bool BallMode => BallRef;

    [SerializeField] private VisualEffect _visualEffect;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _acceleration = 10;
    [SerializeField] private float _gravity = 10;
    [SerializeField] private float _rollSpeed = 300;
    [SerializeField] private float _rollAcceleration = 150;
    [SerializeField] private float _torqueMultiplier = 100;
    [SerializeField] private float _turnSpeed = 10f;
    [SerializeField] private float _interactionRadius = 3f;
    [SerializeField] private string _enterBallEventName;
    [SerializeField] private float _verticleOffsetInBall = 0.05f;

    [SerializeField] private Animator hamAnimator;
    
    [NonSerialized] public GameObject BallRef;
    public int pid;
    private Transform camTrans;

    [SerializeField] private MeshRenderer hamMeshRenderer;

    private void Awake()
    {
        camTrans = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
        _movementInput = moveInput;
    }

    public void OnMovement(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnInteract()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, _interactionRadius);
        foreach (Collider c in col)
        {
            if (c.tag == "HamsterBall")
            {
                BallRef = c.gameObject;
                BallRef.tag = "PlayerHamsterBall";
                
                if (BallRef)
                    OnMount();
                else 
                    OnUnmount();
               
                hamAnimator.SetTrigger("Interact");
                
                if(_visualEffect)
                    _visualEffect.SendEvent(_enterBallEventName);
        
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 up = Vector3.up;
        Vector3 right = Camera.main.transform.right;
        Vector3 forward = Vector3.Cross(right, up);
        Vector3 moveInput_CamOffset = forward * _moveInput.y + right * _moveInput.x;
        
        if (BallMode)
        {
            BallTorque();
            // align ham position with the ball
            transform.position = BallRef.transform.position - Vector3.up * _verticleOffsetInBall;
            // _ball.transform.GetComponent<BallController>().OnMove(_moveInput);
        }
        else
        {
            // Ham movement
            Vector3 targetSpeed = moveInput_CamOffset * _speed;
            Vector3 currSpeed = _rb.velocity;
            Vector3 finalAcceleration = (targetSpeed - currSpeed) * _acceleration;
            finalAcceleration.y = -_gravity;
            _rb.AddForce(finalAcceleration);
        }
        
        // ham turn
        if (_moveInput.magnitude >= 0.1f)
        {
            Vector3 LookDirection = new Vector3(moveInput_CamOffset.x, 0f, moveInput_CamOffset.y).normalized;
            float turnDirection = -Mathf.Sign(Vector3.Cross(moveInput_CamOffset, transform.forward).y);
            float turnMagnitude = 1f - Mathf.Clamp01(Vector3.Dot(transform.forward, LookDirection));
            float angularVelocity = turnMagnitude * _turnSpeed * turnDirection;
            _rb.angularVelocity = new Vector3(0f, angularVelocity, 0f);
        }
    }

    public void BallTorque()
    {
        Vector3 InputVec = _moveInput.x * camTrans.right + _moveInput.y * camTrans.forward;
        
        Rigidbody rb = BallRef.GetComponent<Rigidbody>();
        Vector3 targetAcceleration = InputVec.normalized * _rollSpeed;
        Vector3 currentAcceleration = rb.velocity;
        Vector3 finalAcceleration = (targetAcceleration - currentAcceleration) * _rollAcceleration;
        Vector3 Torque = Vector3.Cross(Vector3.up, finalAcceleration);
        rb.AddTorque(Torque * _torqueMultiplier);
    }

    //DEPRECATED check OnInteract
    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("HamsterBall"))
        // {
        //     BallRef = collision.gameObject;
        //     OnMount();
        // }
    }

    public void OnMount()
    {
        Debug.Log($"{transform.name} is riding {BallRef.name}");
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), BallRef.GetComponent<Collider>(), true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        // hide Ham mesh
        // GetComponent<PlayerModelManager>().HideObject(true, Vector3.zero);
    }

    public void OnUnmount()
    {
        hamMeshRenderer.enabled = true;
        Destroy(BallRef);
        BallRef = null;
    }
}