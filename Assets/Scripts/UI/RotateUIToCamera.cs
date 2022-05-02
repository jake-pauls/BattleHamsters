using Cinemachine;
using UnityEngine;

public class RotateUIToCamera : MonoBehaviour
{
    [SerializeField]
    private bool _bobCanvas = false; 
    [SerializeField]
    private float _idleBobbingSpeed = 2f;
    [SerializeField]
    private float _idleBobbingHeight = 1.1f;

    [SerializeField]
    private CinemachineVirtualCamera _gameCamera = null;

    private void Start() => _gameCamera = CoreCamera.Reference;

    private void Update() {
        transform.LookAt(transform.position + _gameCamera.transform.rotation * Vector3.forward, _gameCamera.transform.rotation * Vector3.up);        

        if (_bobCanvas) {
            Vector3 pos = transform.position;
            float newYPos = Mathf.Sin(Time.time * _idleBobbingSpeed) + 6f;
            transform.position = new Vector3(pos.x, newYPos * _idleBobbingHeight, pos.z);
        }
    }
}