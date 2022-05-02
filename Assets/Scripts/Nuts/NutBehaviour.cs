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
}
