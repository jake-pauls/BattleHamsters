using UnityEngine;

public class NutBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject _parentNut;

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            Debug.Log(col.gameObject.name);
            col.gameObject.GetComponent<PlayerController>().NutCount++;
            Destroy(_parentNut);
        }
    }
}
