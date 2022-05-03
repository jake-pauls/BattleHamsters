using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour
{
    [SerializeField] private PhotonView[] _prefab;
    private int _prefabIndex = 0;

    public void Spawn()
    {
        // PhotonNetwork.Instantiate()
        // PhotonNetwork.InstantiateRoomObject()

        // Instantiate creates an object tied the player that spawned it, when that player quits/disconnects, the object is destroyed
        // InstantiateRoomObject creates an object tied to the level, when the player quits/disconnects it persists
        GetComponent<PhotonView>().RPC("updatePrefabIndex", RpcTarget.All);
        PhotonNetwork.Instantiate(_prefab[_prefabIndex].name, transform.position, transform.rotation);
        _prefabIndex++;
    }

    [PunRPC]
    public void updatePrefabIndex(int index)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _prefabIndex = index;
        }
    }

}
