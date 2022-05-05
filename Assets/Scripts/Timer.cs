using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
    [SerializeField]
    private ScoreManager _scoreManager;

    [Header("Time Variables")]
    [Tooltip("Set this to determine the game length (seconds)")]
    [SerializeField]
    private float _gameLength = 10;
    [SerializeField]
    [Tooltip("In the case of a tie, the following time will be added to the clock")]
    private float _addedTimeForTies = 11;

    [Header("Events")]
    [Space]
    public UnityEvent GameOver;

    [HideInInspector]
    public bool StartTimer = false;

    // Variable to tick down the Timer from the game length
    private float _timeRemaining;

    private void Awake() => _timeRemaining = _gameLength; 

    private void Update() {
        if (StartTimer) {
            if (_timeRemaining > 0) {
                _timeRemaining -= Time.deltaTime;
            } else if (_timeRemaining == 20) {
                // Crazy nut time mode!
            } else {
                if (_scoreManager.DoesWinnerExist()) {
                    Debug.Log("Game is over");
                    GameOver.Invoke();

                    _timeRemaining = 0;
                    StartTimer = false;
                } else {
                    _timeRemaining += _addedTimeForTies;
                }
            }
        }
    }

    public string GetTimeString() {
        float minutes = Mathf.FloorToInt(_timeRemaining / 60);
        float seconds = Mathf.FloorToInt(_timeRemaining % 60);

        if (minutes == 0.0f) {
            return string.Format("{00}", seconds);
        }

        return string.Format("{00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer() => _timeRemaining = _gameLength;
}
