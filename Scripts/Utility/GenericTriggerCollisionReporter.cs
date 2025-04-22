using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerCollisionReporter : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionLayer;

    [SerializeField] private UnityEvent OnTriggered = new UnityEvent();
    [SerializeField] private UnityEvent OnStayed = new UnityEvent();
    [SerializeField] private UnityEvent OnExited = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _collisionLayer) != 0)
        {
            OnTriggered?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _collisionLayer) != 0)
        {
            OnExited?.Invoke();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _collisionLayer) != 0)
        {
            OnStayed?.Invoke();
        }
    }
}
