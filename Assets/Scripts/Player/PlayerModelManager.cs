using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelManager : MonoBehaviour
{
    public void HideObject(bool hideHamster, Vector3 spawnPosition)
    {   
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(!hideHamster);
            if (hideHamster) {
                transform.GetChild(i).transform.position = spawnPosition;
            }
            hideHamster =  !hideHamster;
        }

    }
}
