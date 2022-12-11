using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using Rewired;

public class RewiredPlayerManager : MonoBehaviour
{
    private static RewiredPlayerManager _instance;
    public static RewiredPlayerManager Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<RewiredPlayerManager>();

            return _instance;
        }
    }

    [Header("Camera Management")]
    [SerializeField]
    private CinemachineTargetGroup _targetGroup = null;

    [HideInInspector]
    public static List<GameObject> PlayersInGame = new List<GameObject>();

    private static List<Transform> _spawnPoints = new List<Transform>();

    private int _nextSpawnIndex = 0;

    private CharacterSwitcher _characterSwitcher = null;

    private void Awake() 
    {
        _characterSwitcher = GetComponent<CharacterSwitcher>();

        // Subscribe to ReWired events
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

    private void SetupPlayerCameras(GameObject playerInstance) => _targetGroup.AddMember(playerInstance.transform, 1, 2);

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

    public static Transform SpawnPoint(int pid) => _spawnPoints[pid];

    private void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    private void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }
}
