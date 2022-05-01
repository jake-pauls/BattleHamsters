using UnityEngine;

public class NutBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject _parentNut;

    private PlayerController _playerController = null;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            _playerController = col.gameObject.GetComponent<PlayerController>();
            _playerController.NutCount++;
        
            // Scale player speed as nuts are collected
            _playerController.PlayerSpeed -= 0.5f;
            Destroy(_parentNut);
        }
    }
}
