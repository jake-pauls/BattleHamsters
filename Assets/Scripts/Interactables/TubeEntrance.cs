using System;
using System.Collections;
using System.Collections.Generic;
using Interactables;
using UnityEngine;
using UnityEngine.Assertions;

public class TubeEntrance : MonoBehaviour, IInteractable
{
    private Tube _parentTube;
    [SerializeField] bool isHead = false;

    private void Awake()
    {
        _parentTube = GetComponentInParent<Tube>();
        Assert.IsNotNull(_parentTube, $"{this.name} cannot find the tube in parent: {transform.parent}");
    }

    //todo if a ham -> pop up a UI
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} is NEAR the tube area");
        
        if(IsInteractable(other.gameObject))
            TryInteract(other.gameObject);
    }

    //todo remove the pop-up UI
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{other.gameObject.name} is LEAVING the tube area");        
    }

    public bool IsInteractable(GameObject source)
    {
        //todo validate hamster
        // 1. check player tag
        // 2. check whether in ball form
        if (source.tag != "Player") {
            return _parentTube.IsOccupied;
        }

        if (null != source.transform.parent && source.transform.parent.tag == "PlayerHamsterBall") {
            return _parentTube.IsOccupied;
        }

        return !_parentTube.IsOccupied;
    }

    public bool TryInteract(GameObject source)
    {
        _parentTube.OnInteract_Unsafe(source, isHead);
        return true;
    }
}
