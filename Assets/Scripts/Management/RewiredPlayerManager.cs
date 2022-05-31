using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using Rewired;

public class RewiredPlayerManager : MonoBehaviour
{

    private static RewiredPlayerManager instance;
    public static RewiredPlayerManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<RewiredPlayerManager>();

            return instance;
        }
    }

    [Header("Camera Management")]
    [SerializeField]
    private CinemachineTargetGroup _targetGroup = null;

    private static List<Transform> _spawnPoints = new List<Transform>();
    private int _nextSpawnIndex = 0;

    private CharacterSwitcher _characterSwitcher = null;

    private void Awake() {
        _characterSwitcher = GetComponent<CharacterSwitcher>();
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
    }
    public static void AddSpawnPoint(Transform transform)
    {
        _spawnPoints.Add(transform);

        _spawnPoints = _spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => _spawnPoints.Remove(transform);

    private void SetupPlayerCameras(GameObject playerInstance) =>
        _targetGroup.AddMember(playerInstance.transform, 1, 2);

    private void SpawnPlayer(GameObject playerInstance)
    {
        Transform spawnPoint = _spawnPoints.ElementAtOrDefault(_nextSpawnIndex);

        if (spawnPoint == null)
        {
            Debug.LogError("Missing spawn point for player #" + (_nextSpawnIndex + 1));
            return;
        }

        // CharacterController must be disabled when manually manipulating a character transform
        playerInstance.GetComponent<BallRotation>().enabled = false;
        playerInstance.transform.position = _spawnPoints[_nextSpawnIndex].position;
        playerInstance.transform.rotation = _spawnPoints[_nextSpawnIndex].rotation;
        playerInstance.GetComponent<BallRotation>().enabled = true;

        // Increment so player position moves to the next spawn
        _nextSpawnIndex++;
    }

    //public void OnPlayerJoined(PlayerInput playerInput)
    //{
    //    var spawnedPlayer = playerInput.gameObject;

    //    // Give spawned player an id based on their spawn index [0 -> 3]
    //    spawnedPlayer.GetComponent<BallRotation>().pid = _nextSpawnIndex;

    //    SpawnPlayer(spawnedPlayer);

    //    // Setup other game features (ie: HUDs/UI on a player-by-player basis here)

    //    SetupPlayerCameras(spawnedPlayer);

    //    // Increment the character index to spawn the next character
    //    _characterSwitcher.TriggerNextSpawnCharacter();
    //}

    public Transform SpawnPoint(int pid)
    {
        return _spawnPoints[pid];
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }
}
