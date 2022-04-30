using System;
using System.Collections;
using System.Collections.Generic;
using Interactables;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Spline))]
public class Tube : MonoBehaviour
{
    [SerializeField] private Spline _spline;
    [SerializeField] private float splineDuration = 1;

    private SphereCollider entranceA;
    private SphereCollider entranceB;
    [NonSerialized] public GameObject _currentHam = null;

    private void Awake()
    {
        //WARNING: always 2 sphere colliders represents the sides of the tube
        var sphereColliders = GetComponentsInChildren<SphereCollider>();
        Assert.IsTrue(sphereColliders.Length == 2);
        // align up the entrances' positions
        entranceA = sphereColliders[0];
        entranceA.transform.position = _spline.GetPosition(0);
        entranceB = sphereColliders[1];
        entranceB.transform.position = _spline.GetPosition(1);

        _spline = GetComponent<Spline>();
    }

    public void OnInteract(GameObject hamObj, bool fromASide)
    {
        Tween.Spline(_spline, hamObj.transform, fromASide ? 0 : 1, fromASide ? 1 : 0, true, splineDuration, 0);
    }

    public bool IsOccupied => !_currentHam && _spline;
}