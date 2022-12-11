using System.Collections.Generic;
using UnityEngine;

public class WinnerMenuManager : MonoBehaviour {
    [Header("UI")]
    public List<GameObject> WinnerBanners = new List<GameObject>();

    public void DisplayWinnerMenu(int playerNumber) => WinnerBanners[playerNumber].SetActive(true);
}
