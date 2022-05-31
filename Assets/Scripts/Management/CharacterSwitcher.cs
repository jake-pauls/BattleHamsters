using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour {
    [Header("Characters")]
    [SerializeField]
    private List<GameObject> _availableCharacters = new List<GameObject>();

    private int _characterIndex = 0;

    private void Awake() {
        _characterIndex++;
    }

    public void TriggerNextSpawnCharacter() {
        _characterIndex++;
    }
}
