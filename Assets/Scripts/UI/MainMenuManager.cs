using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MainMenuManager : MonoBehaviour {
    [Header("Cinemachine")]
    [SerializeField]
    private CinemachineVirtualCamera _gameVCam;
    [SerializeField]
    private CinemachineVirtualCamera _menuVCam;
    [Header("UI Elements")]
    [SerializeField]
    private GameObject _mainMenuUI;
    [SerializeField]
    private GameObject _dashboardUI;
    [SerializeField]
    private Button _startButton;
    [Header("Managers")]
    [SerializeField]
    private GameObject _playerManager;
    [SerializeField]
    private DashboardManager _dashboardManager;

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

    public static void Quit() => Application.Quit(); 
}
