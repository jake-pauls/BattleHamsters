using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsManager : MonoBehaviour
{
    private Vector3 ballSpawn = new Vector3(74.5f, 40, 14.6f);
    private float _spawnRandomnessOffset = 10f;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HamsterBall"))
        {
            var spawnRandomness = new Vector3(
                ballSpawn.x + Random.Range(-_spawnRandomnessOffset, _spawnRandomnessOffset),
                ballSpawn.y,
                ballSpawn.z + Random.Range(-_spawnRandomnessOffset, _spawnRandomnessOffset));
            other.transform.position = spawnRandomness;
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
        }
        if (other.CompareTag("Player"))
        {
            if (true == other.GetComponent<BallRotation>()._ballMode)
            {
                GameObject ball = other.gameObject.transform.parent.gameObject;
                other.GetComponent<BallRotation>().OnUnmount();
                var spawnRandomness = new Vector3(
                    ballSpawn.x + Random.Range(-_spawnRandomnessOffset, _spawnRandomnessOffset),
                    ballSpawn.y,
                    ballSpawn.z + Random.Range(-_spawnRandomnessOffset, _spawnRandomnessOffset));
                ball.transform.position = spawnRandomness;
                ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
            other.transform.position = RewiredPlayerManager.SpawnPoint(other.GetComponent<BallRotation>().playerId).position;
            other.GetComponent<HamsterHealth>().revivePayer();
        }
        if (other.CompareTag("Nut"))
        {
            Destroy(other.gameObject);
        }

    }
}
