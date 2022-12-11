using UnityEngine;
using TMPro;

public class DashboardManager : MonoBehaviour {
    [Header("Managers")]
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private Timer _timer;
    [Header("Dashboard Content")]
    [SerializeField]
    private TMP_Text _gameTimerUI;
    [SerializeField]
    private TMP_Text[] _playerScoreUIs;

    private void Awake() => ResetDashboard();

    private void Update() {
        _gameTimerUI.text = _timer.GetTimeString();

        if (_playerScoreUIs.Length == _scoreManager.ScoreAreas.Length) {
            for (int i = 0; i < _playerScoreUIs.Length; i++) {
                _playerScoreUIs[i].text = _scoreManager.ScoreAreas[i].CurrentScore.ToString();
            }
        }
    }

    public void ResetDashboard() {
        _timer.ResetTimer();
        _timer.StartTimer = true;
    }
}
