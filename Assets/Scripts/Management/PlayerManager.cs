using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public static PlayerManager Instance 
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<PlayerManager>();
            
            return instance;
        }
    }

    [Header("Camera Management")] 
    [SerializeField]
    private CinemachineTargetGroup _targetGroup = null;

    [HideInInspector]
    public List<GameObject> PlayersInGame = new List<GameObject>();

    private static List<Transform> _spawnPoints = new List<Transform>();
    private int _nextSpawnIndex = 0;

    private CharacterSwitcher _characterSwitcher = null;

    private void Awake() => _characterSwitcher = GetComponent<CharacterSwitcher>();

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

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        var spawnedPlayer = playerInput.gameObject;

        // Add spawned player to list of players in game
        PlayersInGame.Add(spawnedPlayer);

        // Give spawned player an id based on their spawn index [0 -> 3]
        spawnedPlayer.GetComponent<BallRotation>().pid = _nextSpawnIndex;

        SpawnPlayer(spawnedPlayer);

        // Setup other game features (ie: HUDs/UI on a player-by-player basis here)

        SetupPlayerCameras(spawnedPlayer);

        // Increment the character index to spawn the next character
        _characterSwitcher.TriggerNextSpawnCharacter();
    }

    public Transform SpawnPoint(int pid) => _spawnPoints[pid];
}