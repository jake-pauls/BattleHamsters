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
    [SerializeField]
    private float _spawnRandomnessOffset = 3.5f;

    // Spawn vector
    private Vector3 _spawnRandomness;

    private bool _canSpawnNut = false;

    public int NumberOfSpawnedNuts = 0;

    private void Awake() => _canSpawnNut = true;

    private void Update() {
        if (!UIManager.IsMainMenuUp) {
            if (_canSpawnNut) {
                SpawnNut();
            }
        }
    }

    private void SpawnNut() {
        _canSpawnNut = false;
        _spawnRandomness = new Vector3(Random.Range(-_spawnRandomnessOffset, _spawnRandomnessOffset),
            0,
            Random.Range(-_spawnRandomnessOffset, _spawnRandomnessOffset));
        if (NumberOfSpawnedNuts < _maxNuts) {
            Instantiate(_nutPrefab, _nutSpawnPoint.position + _spawnRandomness, _nutSpawnPoint.rotation * Random.rotation);
            NumberOfSpawnedNuts++;
        }

        StartCoroutine(NutCooldown());
    }

    public void CollectNut() => NumberOfSpawnedNuts--;

    IEnumerator NutCooldown() {
        yield return new WaitForSeconds(_spawnRate);

        _canSpawnNut = true;
    }

    private void OnDrawGizmos() {
        // Yellow: Nut Spawn Location
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_nutSpawnPoint.position, new Vector3(5.0f, 1.0f, 5.0f));
    }
}
