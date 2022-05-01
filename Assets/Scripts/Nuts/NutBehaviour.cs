using UnityEngine;

public class NutBehaviour : MonoBehaviour {
    [SerializeField]
    private NutFunctions nf;

    private PlayerController _playerController = null;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player" && nf._canTriggerFind) {
            
                nf.OnAttachedToPlayer();
            nf.SetTargetBool(true);
            nf.SetTargetLocation();

            _playerController = col.gameObject.GetComponent<PlayerController>();
            _playerController.NutCount++;
        
            // Scale player speed as nuts are collected
            _playerController.PlayerSpeed *= 0.5f;
            this.enabled = false;
        }
    }
}
