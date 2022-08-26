using UnityEngine;

public class NutBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject _parentNut;

    private BallRotation _playerController = null;

    [SerializeField] private System.Collections.Generic.List<AudioClip> _nutPickupSound;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player" && null == col.gameObject.transform.parent) {
            _playerController = col.gameObject.GetComponent<BallRotation>();
            _playerController.NutCount++;
            int index = Mathf.Min(_playerController.NutCount, _nutPickupSound.Count - 1);
            SoundManager.Instance.PlaySound(_nutPickupSound[index]);
            // Scale player speed as nuts are collected
            float lerpSpeed = _playerController._maxSpeed - Mathf.Lerp(0, _playerController._maxSpeed, _playerController.NutCount / _playerController._maxSpeed) + 1;
            _playerController._speed = lerpSpeed;
            Destroy(_parentNut);
        }
    }
}
