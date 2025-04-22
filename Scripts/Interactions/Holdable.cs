using System;
using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Holdable : Interactable
{
    [Tooltip("If the object is [x] units from starting point, return it to starting point")]
    [SerializeField] private bool returnToSender = true;
    
    [Header("Rigidbody")]
    [SerializeField] private float drag = 0f;
    [SerializeField] private float angularDrag = 0.05f;
    public Rigidbody Rigidbody => _rigidbody;
    protected Rigidbody _rigidbody;
    protected RotationConstraint _rotationConstraint;

    public Action<Holdable> OnHeldCallback = (interactable) => { };
    public Action<Holdable> OnDroppedCallback = (interactable) => { };

    protected override void OnStart()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody == null)
        {
            Debug.LogWarning($"No rigidbody on holdable {gameObject.name}, adding one.");
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        
        _rigidbody.linearDamping = drag;
        _rigidbody.angularDamping = angularDrag;

        if (returnToSender)
            gameObject.AddComponent<ReturnToSender>();
    }
    
    public bool IsHeld;

    protected abstract void OnHeld();
    public void Held()
    {
        IsHeld = true;
        OnHeld();
        
        OnHeldCallback.Invoke(this);
    }
    
    protected abstract void OnDropped();
    public void Dropped()
    {
        IsHeld = false;
        OnDropped();
        
        OnDroppedCallback.Invoke(this);
    }

    protected override void OnInteracted()
    {
        CameraInteractor.Instance.PickUp(this);
    }
}
