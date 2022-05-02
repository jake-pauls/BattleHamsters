using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour {
    private void Awake() => PlayerManager.AddSpawnPoint(transform);

    private void OnDestroy() => PlayerManager.RemoveSpawnPoint(transform);

    private void OnDrawGizmos() {
        // Cyan: Spawn Location
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.75f);

        // Green: Spawn Forward Direction
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
