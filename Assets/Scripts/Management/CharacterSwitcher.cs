using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour {
    [Header("Characters")]
    [SerializeField]
    private List<GameObject> _availableCharacters = new List<GameObject>();

    private PlayerInputManager _playerInputManager = null;

    private int _characterIndex = 0;

    private void Awake() {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.playerPrefab = _availableCharacters[_characterIndex];
        _characterIndex++;
    }

    public void TriggerNextSpawnCharacter() {
        _playerInputManager.playerPrefab = _availableCharacters[_characterIndex];
        _characterIndex++;
    }
}
