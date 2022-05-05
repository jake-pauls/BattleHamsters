using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

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
    private GameObject _playerManager;
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
    
    private void Awake() => SwitchToMainMenu(); 

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

        // Start looking for players
        _playerManager.SetActive(true);
    }

    public void SwitchToMainMenu() {
        IsMainMenuUp = true;

        // Transition to menu camera
        _menuVCam.Priority = 1;
        _gameVCam.Priority = 0;

        // Display main menu and deactivate player spawning
        _mainMenuUI.SetActive(true);
        _dashboardUI.SetActive(false);
        _playerManager.SetActive(false);
    }

    public void SwitchToWinnerMenu() {
        // Transition to player camera
        Debug.Log("Display winner menu");

        // Display winner UI and player banner
        _winnerMenuUI.SetActive(true);

        // Retrieves a descending list of player scores, displays the menu for the top entry (winner) 
        List<KeyValuePair<int, int>> playerScores = _scoreManager.GetPlayerInformation().OrderByDescending(x => x.Value).ToList();
        _winnerMenuUI.GetComponent<WinnerMenuManager>().DisplayWinnerMenu(playerScores[0].Key);

        _dashboardUI.SetActive(false);
        _playerManager.SetActive(false);
    }

    public void PlayAgain() => Debug.Log("Play again!");

    public static void Quit() => Application.Quit(); 
}
