using UnityEngine;

public class ScoreArea : MonoBehaviour { 
    [SerializeField]
    private NutManager _nutManager = null;

    private BallRotation _playerReference = null;

    public int CurrentScore = 0;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            _playerReference = col.gameObject.GetComponent<BallRotation>();

            CurrentScore += _playerReference.NutCount;

            for (int i = 0; i < _playerReference.NutCount; i++) {
                _nutManager.CollectNut();
            }
            _playerReference.DropAllNuts(false);
        }
    }
}
