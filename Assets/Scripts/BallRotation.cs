using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Photon.Pun;

public class BallRotation : MonoBehaviour {
    public bool inputDisabled;

    private Vector2 _moveInput;
    private Vector3 _movementInput;
    private Rigidbody _rb;
    public bool _ballMode;

    public float _maxSpeed = 10f; // if changing speed please do in code
    public float _speed = 10f; // if changing speed please do in code
    [SerializeField] private float _acceleration;
    [SerializeField] private float _gravity;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollAcceleration;
    [SerializeField] private float _torqueMultiplier;
    [SerializeField] private float _interactionRadius = 3f;
    [SerializeField] public ParticleSystem dustTrail;
    private bool _groundedPlayer;
    public GameObject _ball;
    [SerializeField] private VisualEffect _enterBallEffect;
    [SerializeField] private string _enterBallEventName;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private Vector3 _ballOffset;
    private float _turnSpeed = 10f;
    public int pid;
    public Vector3 ballOffset = new Vector3(0, -1f, 0);
    private HamsterHealth hamHealth;
    private PhotonView _view;   

    [Header("Nut Management")]
    public int NutCount;
    [SerializeField]
    private GameObject NutPrefab;
    [SerializeField]
    private Text _nutCountText;

    private void Awake() {
        hamHealth = GetComponent<HamsterHealth>();        
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _view = GetComponent<PhotonView>(); 
        dustTrail.gameObject.SetActive(false);
    }

    // Photon instantiation does not call start function


    // Update is called once per frame
    void Update() {
        Vector3 moveInput = new Vector3(-_moveInput.x, 0f, -_moveInput.y).normalized;
        _movementInput = moveInput;

        _nutCountText.text = NutCount.ToString();
    }

    public void OnMove(InputValue value) {
        _moveInput = inputDisabled ? Vector2.zero : value.Get<Vector2>();
    }

    private void FixedUpdate() {
        if (!GetComponent<PhotonView>().IsMine) return;
        if (_ballMode) {
                //transform.position = _ball.transform.position + ballOffset;
            if (_ball.GetComponent<PhotonView>().IsMine)
            {
                BallTorque();
            }
                dustTrail.gameObject.SetActive(true);
        } else {
            Vector3 up = Vector3.up;
            Vector3 right = Camera.main.transform.right;
            Vector3 forward = Vector3.Cross(right, up);
            Vector3 moveInput = forward * _moveInput.y + right * _moveInput.x;
            Vector3 targetAcceleration = moveInput * _speed;
            Vector3 currentAcceleration = _rb.velocity;
            Vector3 finalAcceleration = (targetAcceleration - currentAcceleration) * _acceleration;
            finalAcceleration.y = -_gravity;
            _rb.AddForce(finalAcceleration);
        }

        Vector3 LookDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
        float turnDirection = -Mathf.Sign(Vector3.Cross(_movementInput, transform.forward).y);
        float turnMagnitude = 1f - Mathf.Clamp01(Vector3.Dot(transform.forward, LookDirection));
        if (_moveInput.magnitude < 0.1f) turnMagnitude = 0f;
        float angularVelocity = turnMagnitude * _turnSpeed * turnDirection;
        _rb.angularVelocity = new Vector3(0f, angularVelocity, 0f);
    }


    public void BallTorque() {
        Rigidbody rb = _ball.GetComponent<Rigidbody>();
        Vector3 targetAcceleration = _movementInput * _rollSpeed;
        Vector3 currentAcceleration = rb.velocity;
        Vector3 finalAcceleration = (targetAcceleration - currentAcceleration) * _rollAcceleration;
        Vector3 Torque = Vector3.Cross(Vector3.up, finalAcceleration);
        rb.AddTorque(Torque * _torqueMultiplier);
    }

    [PunRPC]
    public void OnMount() {
        gameObject.transform.SetParent(_ball.transform);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), _ball.GetComponent<Collider>(), true);
        gameObject.transform.localPosition = Vector3.zero + _ballOffset;
        //gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.rotation = _ball.transform.rotation;
        _ballMode = true;
        if (_view.IsMine)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        _ball.tag = "PlayerHamsterBall";
    }

    [PunRPC]
    public void OnUnmount() {
        if (!_ball) return;
        gameObject.transform.SetParent(null);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), _ball.GetComponent<Collider>(), false);
        _ballMode = false;

        if (_view.IsMine)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _ball.tag = "HamsterBall";

        StartCoroutine(resetRotation());
    }

    IEnumerator resetRotation() {
        yield return new WaitForSeconds(0.5f);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
    }

    public void OnJump() {
        if (inputDisabled) return;

        if (!_ballMode && IsGrounded()) {
            _rb.AddForce(Vector3.up * _jumpHeight);
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }



    public void OnInteract() {
        if (inputDisabled) return;

        if (!_ballMode && IsGrounded()) {
            Collider[] col = Physics.OverlapSphere(transform.position, _interactionRadius);
            foreach (Collider c in col) {
                if (c.tag == "HamsterBall") {
                    int viewID = c.GetComponent<PhotonView>().ViewID;
                    //_ball = c.gameObject;
                    _view.RPC("SetBall", RpcTarget.All,viewID);
                    if (_view.IsMine)
                    {
                        _view.RPC("OnMount", RpcTarget.All);
                    }

                    GetComponent<Animator>().SetTrigger("Interact");
                    if (_enterBallEffect) _enterBallEffect.SendEvent(_enterBallEventName);
                    dustTrail.Stop();
                    dustTrail.Play();
                    break;
                }
            }
        }
        else if (_ballMode) {

            if (_view.IsMine) _view.RPC("OnUnmount", RpcTarget.All);
            dustTrail.Stop();
            dustTrail.Play();
            
        }
    }

    [PunRPC]
    public void SetBall(int viewID)
    {
        _ball = PhotonView.Find(viewID).gameObject;
    }

    public void OnDropNut() {
        if (NutCount > 0) {
            // Drop Nut
            float lerpSpeed = _maxSpeed - Mathf.Lerp(0, _maxSpeed, NutCount / _maxSpeed) + 1;
            _speed = lerpSpeed;

            NutCount--;
            Vector3 newPos = transform.position - (transform.forward * -1 * 2.0f);
            GameObject newNut = Instantiate(NutPrefab, newPos, Quaternion.identity);
            newNut.GetComponent<Rigidbody>().AddExplosionForce(300f, transform.position, 5.0f, 3.0f);
        }
    }

    public void DropAllNuts()
    {
        while (NutCount != 0)
        {
            OnDropNut();
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Rigidbody otherrb = collision.rigidbody;
    //         // kewk
    //         FlattenHamster(otherrb.gameObject);
    //     }
    // }
    //
    // public void FlattenHamster(GameObject otherHamster)
    // {
    //     // Kewk
    //     otherHamster.gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 1f);
    // }
}