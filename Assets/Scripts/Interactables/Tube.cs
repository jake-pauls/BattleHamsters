using System;
using System.Collections;
using System.Collections.Generic;
using Interactables;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Spline))]
public class Tube : MonoBehaviour
{
    private Spline _spline;
    [SerializeField] private float splineDuration = 1;

    [SerializeField] SphereCollider entranceA;
    [SerializeField] SphereCollider entranceB;
    [NonSerialized] public GameObject _currentHam = null;

    private void Awake()
    {
        _spline = GetComponent<Spline>();

        //WARNING: always 2 sphere colliders represents the sides of the tube
        var sphereColliders = GetComponentsInChildren<SphereCollider>();
        Assert.IsTrue(sphereColliders.Length == 2);
        // align up the entrances' positions
        entranceA = sphereColliders[0];
        entranceA.transform.position = _spline.GetPosition(0);
        entranceB = sphereColliders[1];
        entranceB.transform.position = _spline.GetPosition(1);
    }

    public void OnInteract_Unsafe(GameObject hamObj, bool fromASide)
    {
        _currentHam = hamObj;
        
        Tween.Spline(_spline, hamObj.transform, fromASide ? 0 : 1, fromASide ? 1 : 0, true, splineDuration, 0);
        Debug.Log("moving");
        
        //todo disable input, physics and collision check perhaps
        //todo WARNING: do not disable PlayerInput, otherwise it triggers the respawn
        // hamObj.GetComponent<PlayerInput>().enabled = false;

        StartCoroutine(DelayedResetHamState(hamObj, splineDuration));
    }

    private IEnumerator DelayedResetHamState(GameObject hamObj, float inSplineDuration)
    {
        yield return new WaitForSeconds(inSplineDuration);

        OnInteractionFinished(hamObj);
    }

    private void OnInteractionFinished(GameObject hamObj)
    {        
        Assert.IsNotNull(hamObj, "Cannot reset player input, since hamObj is null.");
        
        //todo reset input, physics and collision check perhaps
        // hamObj.GetComponent<PlayerInput>().enabled = true;
    }
    
    public bool IsOccupied => _currentHam;
}