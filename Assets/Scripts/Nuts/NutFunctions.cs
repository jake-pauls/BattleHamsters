using System.Collections;
using UnityEngine;

public class NutFunctions : MonoBehaviour
{
    [SerializeField] private float _detectPlayerRadius =3f;
    [SerializeField] private float _pickUpHeightScale =0.4f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _dropHeight = 5f;
    [SerializeField] private float _dropRadius = 5f;
    [SerializeField] private float _gainRadius = 1f;
    private Transform _targetTransform;
    private int _targetindex;
    [SerializeField]private float _timeBeforeAbleToPickAgain = 5f;
    private IEnumerator _coroutine;
    public bool _canTriggerFind = true;
    private float _dropForceScale;
    private BallRotation _playerController = null;

    public void NutsPickUp()
    {
        //takes nut position and lerps between nut and player
        _coroutine =PickUpAnimation((transform.position - _targetTransform.transform.position).magnitude);
        StartCoroutine(_coroutine);
    }

    public void HitPlayer()
    {
        //check distance
        if (!((_targetTransform.transform.position - transform.position).magnitude < _gainRadius)) return;

        _playerController = _targetTransform.gameObject.GetComponentInParent<BallRotation>();
        _playerController.NutCount++;
    
        // Scale player speed as nuts are collected
        float percentage = _playerController._speed * 0.15f;
        _playerController._speed -= percentage;

        //stopdrawing nuts toward player
        StopCoroutine(_coroutine);
        //attac to player
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.SetParent(_targetTransform.transform);
        CarryNutPositions cnp =_targetTransform.GetComponentInParent<CarryNutPositions>();
        for (int i = 0; i < cnp.IsOccupied.Length; i ++)
        {
            if (!cnp.IsOccupied[i])
            {
                cnp.IsOccupied[i] = true;
                break;
            }
        }
        
        transform.position = _targetTransform.position;
        transform.rotation = _targetTransform.rotation;
        _canTriggerFind = false;

        //increase nut count
        //_playerController = _targetTransform.GetComponentInParent<PlayerController>();
        //_playerController.NutCount++;

    }

    public void DropNuts()
    {
        CarryNutPositions cnp = _targetTransform.GetComponentInParent<CarryNutPositions>();
        for (int i = cnp.IsOccupied.Length -1; i > -1; i--)
        {
            if (cnp.IsOccupied[i])
            {
                cnp.IsOccupied[i] = false;
                break;
            }
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.SetParent(null);
        Vector3 dropforce = new Vector3(Random.Range(-_dropRadius, _dropRadius), Random.Range(10f, 15f) * _dropHeight, Random.Range(-_dropRadius, _dropRadius));
        GetComponent<Rigidbody>().AddForce(dropforce * _dropForceScale);
        StartCoroutine(resetTrigger());
    }
    IEnumerator PickUpAnimation(float distance)
    {
        while (Vector3.Distance(transform.position, _targetTransform.position) > 2f)
        {
            float currentDistanceToPlayer = (_targetTransform.position - transform.position).magnitude;
            float height = Mathf.Sin(currentDistanceToPlayer * Mathf.PI / distance) * _pickUpHeightScale;
            Vector3 targetFlattenedPosition = (_targetTransform.position - transform.position).normalized;
            Vector3 targetVelocity = new Vector3(targetFlattenedPosition.x, height + targetFlattenedPosition.y, targetFlattenedPosition.z);
            GetComponent<Rigidbody>().AddForce(targetVelocity * _speed);
            yield return null;
        }

    }

    IEnumerator resetTrigger()
    {
        yield return new WaitForSeconds(_timeBeforeAbleToPickAgain);
        _canTriggerFind = true;
    }

    private bool FindPlayer()
    {
        if (!_canTriggerFind) return false;
        //detect any collider in Hamster layer (layer3)
        Collider[] detectColliders = new Collider[4];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _detectPlayerRadius, detectColliders, 1<<3);
        Debug.Log(this.gameObject);

        if (numColliders == 0) return false;
        foreach (Collider c in detectColliders)
        {
            CarryNutPositions cnp = c.gameObject.GetComponent<CarryNutPositions>();
            for (int i = 0; i < cnp.positions.Length; i++)
            {
                if (!cnp.IsOccupied[i])
                {
                    _targetTransform = cnp.positions[i];
                    return true;
                }
            }
        }
            return false;
    }

    private void Update()
    {
        if (FindPlayer()) { 
            NutsPickUp();

            HitPlayer();
        }

        if(Input.GetKeyDown("g"))
        {
            DropNuts();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            StopCoroutine(_coroutine);
        }
    }

    public void OnAttachedToPlayer()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.SetParent(_targetTransform.transform);
    }

    public void SetTargetBool(bool b)
    {
        GetComponentInParent<CarryNutPositions>().IsOccupied[_targetindex] = b;
    }

    public void SetTargetLocation()
    {
        transform.position = GetComponentInParent<CarryNutPositions>().positions[_targetindex].position;
        transform.rotation = GetComponentInParent<CarryNutPositions>().positions[_targetindex].rotation;
    }


}
