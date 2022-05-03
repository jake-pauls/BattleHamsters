using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class DisplayNickName : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
