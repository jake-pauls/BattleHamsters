using UnityEngine;

public class ScoreArea : MonoBehaviour { 
    [SerializeField]
    private NutManager _nutManager = null;

    private PlayerController _playerReference = null;

    public int CurrentScore = 0;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            _playerReference = col.gameObject.GetComponent<PlayerController>();

            CurrentScore += _playerReference.NutCount;

            for (int i = 0; i < _playerReference.NutCount; i++) {
                _playerReference.PlayerSpeed += 0.5f;
                _nutManager.CollectNut();
            }

            _playerReference.NutCount = 0;
        }
    }
}
