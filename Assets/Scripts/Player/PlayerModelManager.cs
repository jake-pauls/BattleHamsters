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
        float x = Random.RandomRange(0f, 1f) < 0.5f ? Random.Range(2, 3) : Random.Range(2, 3) * -1;
        float y = 2;
        float z = Random.RandomRange(0f, 1f) < 0.5f ? Random.Range(2, 3) : Random.Range(2, 3) * -1;
        Vector3 offset = new  Vector3(x, y, z);
        if (!hideHamster) {
            Instantiate(ballObject, this.transform.position + offset, this.transform.rotation);
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
