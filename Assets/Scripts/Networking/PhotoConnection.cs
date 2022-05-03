using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class PhotoConnection : MonoBehaviourPunCallbacks
{
    public UnityEvent OnConnectedToMasterEvent;
    public UnityEvent OnDisconnectedEvent;
    [SerializeField] private string _nickName = "Player" + Random.Range(0, 999).ToString();
    [SerializeField] private string _gameVersion = "0.0.1";

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ConnectToMaster()
    {
        //stop if already connected
        if (PhotonNetwork.IsConnected) return;

        //decide connection settings
        PhotonNetwork.NickName = _nickName;
        PhotonNetwork.GameVersion = _gameVersion;

        //connect

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        OnConnectedToMasterEvent.Invoke();
        Debug.Log("joined Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected:" + cause.ToString());
        OnDisconnectedEvent.Invoke();
    }
}
