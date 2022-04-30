using System.Collections;
using UnityEngine;

public class NutManager : MonoBehaviour {
    [Header("Spawn Data")]
    [SerializeField]
    private int _maxNuts;
    [SerializeField]
    private int _spawnRate;
    [Header("Prefab Information")]
    [SerializeField]
    private GameObject _nutPrefab;
    [SerializeField]
    private Transform _nutSpawnPoint;

    private bool _canSpawnNut = false;

    public int NumberOfSpawnedNuts = 0;

    private void Awake() => _canSpawnNut = true;

    private void Update() {
        if (_canSpawnNut) {
            SpawnNut();
        }
    }

    private void SpawnNut() {
        _canSpawnNut = false;

        if (NumberOfSpawnedNuts < _maxNuts) {
            Instantiate(_nutPrefab, _nutSpawnPoint);
            NumberOfSpawnedNuts++;
        }

        StartCoroutine(NutCooldown());
    }

    public void CollectNut() {
        NumberOfSpawnedNuts--;
    }

    IEnumerator NutCooldown() {
        yield return new WaitForSeconds(_spawnRate);

        _canSpawnNut = true;
    }

    private void OnDrawGizmos() {
        // Yellow: Nut Spawn Location
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_nutSpawnPoint.position, new Vector3(1.0f, 1.0f, 1.0f));
    }
}
