using System;
using System.Collections;
using System.Collections.Generic;
using Interactables;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Spline))]
public class LaunchPad : MonoBehaviour, IInteractable
{
    private Spline _spline;
    [SerializeField] private float translateDuration = 2f;

    private void Awake()
    {
        _spline = GetComponent<Spline>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInteractable(other.gameObject))
        {
            TryInteract(other.gameObject);
        }
    }

    public bool IsInteractable(GameObject source)
    {
        //todo validate hamster
        // 1. check player tag
        // 2. check whether in ball form
        return source.CompareTag("Player");
    }

    public bool TryInteract(GameObject source)
    {
        Tween.Spline(_spline, source.transform, 0, 1, true, translateDuration, 0);

        //todo disable input, physics and collision check perhaps
        //source.GetComponent<PlayerInput>().enabled = false;

        StartCoroutine(DelayedResetHamState(source, translateDuration));
        
        return true;
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
        //hamObj.GetComponent<PlayerInput>().enabled = true;
    }
}