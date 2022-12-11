using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;

public class UIManager : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField]
    private GameObject _mainMenuUI;
    [SerializeField]
    private GameObject _dashboardUI;
    [SerializeField]
    private GameObject _winnerMenuUI;
    [Header("Managers")]
    [SerializeField]
    private DashboardManager _dashboardManager;
    [SerializeField]
    private ScoreManager _scoreManager;
    [Header("Cinemachine")]
    [SerializeField]
    private CinemachineVirtualCamera _gameVCam;
    [SerializeField]
    private CinemachineVirtualCamera _menuVCam;

    [HideInInspector]
    public static bool IsMainMenuUp = true;

    private Player player; // The Rewired Player
    public int playerId = 0; // The Rewired player id of this character

    private void Awake() {
        SwitchToMainMenu();
        player = ReInput.players.GetPlayer(playerId);
    }

    public void SwitchToGame() {
        IsMainMenuUp = false;

        // Transition to game camera
        _menuVCam.Priority = 0;
        _gameVCam.Priority = 1;

        // Disable main menu 
        _mainMenuUI.SetActive(false);

        // Open dashboard
        _dashboardUI.SetActive(true);
        _dashboardManager.ResetDashboard();
    }

    public void SwitchToMainMenu() {
        IsMainMenuUp = true;

        // Transition to menu camera
        _menuVCam.Priority = 1;
        _gameVCam.Priority = 0;

        // Display main menu and deactivate player spawning
        _mainMenuUI.SetActive(true);
        _dashboardUI.SetActive(false);
    }

    public void SwitchToWinnerMenu() {

        // Display winner UI and player banner
        _winnerMenuUI.SetActive(true);

        // Retrieves a descending list of player scores, displays the menu for the top entry (winner) 
        List<KeyValuePair<int, int>> playerScores = _scoreManager.GetSortedPlayerInformation();
        var winningPlayerIndex = playerScores[0].Key;
        Debug.Log(winningPlayerIndex);
        _winnerMenuUI.GetComponent<WinnerMenuManager>().DisplayWinnerMenu(winningPlayerIndex);

        // Transition to player camera
        GameObject winningPlayer = RewiredPlayerManager.PlayersInGame.Find(p => p.GetComponent<BallRotation>().playerId == winningPlayerIndex);
        winningPlayer.GetComponentInChildren<CinemachineVirtualCamera>().Priority = 1;
        _gameVCam.Priority = 0;

        _dashboardUI.SetActive(false);
    }

    public void PlayAgain() => Debug.Log("Play again!");

    public static void Quit() => Application.Quit(); 
}
