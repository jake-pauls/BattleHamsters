using UnityEngine;

public class ScoreArea : MonoBehaviour { 
    private PlayerController _playerReference = null;

    [HideInInspector]
    public int CurrentScore = 0;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            _playerReference = col.gameObject.GetComponent<PlayerController>();

            CurrentScore += _playerReference.NutCount;
            _playerReference.NutCount = 0;
        }
    }
}
