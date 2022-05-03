using System.Collections;
using UnityEngine;

public class NutBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject _parentNut;

    private BallRotation _playerController = null;

    private void OnTriggerEnter(Collider col) {
        Debug.Log("Trigger");
        if (col.gameObject.tag == "Player") {
            _playerController = col.gameObject.GetComponent<BallRotation>();
            _playerController.NutCount++;
        
            // Scale player speed as nuts are collected
            float percentage = _playerController._speed * 0.15f;
            _playerController._speed -= percentage;

            Destroy(_parentNut);
        }
    }
        private void FindPlayer()
    {
        //detect any collider in Hamster layer (layer3)
        Collider[] detectColliders = new Collider[4];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 3f, detectColliders, 1<<3);
        Debug.Log(this.gameObject);

        if (numColliders == 0) return;
        {
            StartCoroutine(PickUpAnimation((transform.position - detectColliders[0].transform.position).magnitude,detectColliders[0].gameObject.transform));
        }

    }

        IEnumerator PickUpAnimation(float distance, Transform _targetTransform)
    {
        while (Vector3.Distance(transform.position, _targetTransform.position) > 2f)
        {
            float currentDistanceToPlayer = (_targetTransform.position - transform.position).magnitude;
            float height = Mathf.Sin(currentDistanceToPlayer * Mathf.PI / distance) * 1f;
            Vector3 targetFlattenedPosition = (_targetTransform.position - transform.position).normalized;
            Vector3 targetVelocity = new Vector3(targetFlattenedPosition.x, height + targetFlattenedPosition.y, targetFlattenedPosition.z);
            GetComponentInParent<Rigidbody>().AddForce(targetVelocity);
            yield return null;
        }

    }

    private void Update()
    {
        FindPlayer();
    }
}
