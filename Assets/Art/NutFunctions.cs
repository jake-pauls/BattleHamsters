using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutFunctions : MonoBehaviour
{
    [SerializeField] private float _detectPlayerRadius =3f;
    [SerializeField] private float _pickUpHeightScale =0.4f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _dropHeight = 5f;
    [SerializeField] private float _dropRadius = 5f;
    private Transform _targetTransform;
    private int _targetindex;
    [SerializeField]private float _timeBeforeAbleToPickAgain = 5f;
    private IEnumerator _coroutine;
    public bool _canTriggerFind = true;
    private float _dropForceScale;

    private void Start()
    {
        
    }
    public void NutsPickUp()
    {
        //takes nut position and lerps between nut and player
        _coroutine =PickUpAnimation((transform.position - _targetTransform.transform.position).magnitude);
        StartCoroutine(_coroutine);
    }

    IEnumerator PickUpAnimation(float distance)
    {
        while (Vector3.Distance(transform.position, _targetTransform.position) > 2f && _canTriggerFind)
        {
            float currentDistanceToPlayer = (_targetTransform.position - transform.position).magnitude;
            float height = Mathf.Sin(currentDistanceToPlayer * Mathf.PI / distance) * _pickUpHeightScale;
            Vector3 targetFlattenedPosition = (_targetTransform.position - transform.position).normalized;
            Vector3 targetVelocity = new Vector3(targetFlattenedPosition.x, height, targetFlattenedPosition.z);
            GetComponent<Rigidbody>().AddForce(targetVelocity * _speed);
            yield return null;
        }
        _canTriggerFind = false;

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
        if (FindPlayer()) NutsPickUp();


        if(Input.GetKeyDown("g"))
        {
            DropNuts();
        }
    }

    public void DropNuts()
    {
        SetTargetBool(false);
        GetComponentInChildren<NutBehaviour>().enabled = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.SetParent(null);
        Vector3 dropforce = new Vector3(Random.Range(-_dropRadius, _dropRadius), Random.Range(10f, 15f) * _dropHeight, Random.Range(-_dropRadius, _dropRadius));
        GetComponent<Rigidbody>().AddForce(dropforce * _dropForceScale);
        StartCoroutine(resetTrigger());
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
