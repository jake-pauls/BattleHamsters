using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelManager : MonoBehaviour
{

    public GameObject ballObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideObject(bool hideHamster, Vector3 spawnPosition)
    {   
        if (!hideHamster) {
            Instantiate(ballObject);
        } 
            
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(hideHamster);
            if (hideHamster) {
                transform.GetChild(i).transform.position = spawnPosition;
            }
            hideHamster =  !hideHamster;
        }

    }
}
