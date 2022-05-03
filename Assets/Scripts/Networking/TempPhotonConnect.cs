using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;
using UnityEngine.Events;

public class TempPhotonConnect : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _nickName = "XJP8964";
    [SerializeField] private string _version = "0.01";
    [SerializeField] private byte _maxPlayers = 12;
    [SerializeField] private string _roomName = "Tiananmen";

    public UnityEvent JoinedRoom;

    private void Start()
    {
        ConnectToMaster();
    }

    private void ConnectToMaster()
    {
        // stop if already connected
        if (PhotonNetwork.IsConnected) return;

        // assign connection settings
        PhotonNetwork.LocalPlayer.NickName = _nickName;
        PhotonNetwork.GameVersion = _version;

        // connect
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master, Joining/Creating room: " + _roomName);

        // assign room settings
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayers;

        // join room if exists, otherwise create
        PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}");

        JoinedRoom.Invoke();
    }
}
