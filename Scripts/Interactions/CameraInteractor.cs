using System;
using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using UnityEngine.UI;

public class CameraInteractor : MonoBehaviour
{
    public static CameraInteractor Instance;
    [SerializeField] private Transform _holdPoint;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    [SerializeField] private GameObject _interactableReticle;
    [SerializeField] private GameObject _nonInteractableReticle;

    [SerializeField] private float maxInteractDistance = 100f;
    [SerializeField] private LayerMask interactableLayer;
    
    // for lerp hold
    // [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float _throwForce = 5f;

    private Transform _heldPreviousParent;
    
    private Interactable _currentlyHovered;

    private Interactable CurrentlyHovered
    {
        get => _currentlyHovered;
        set
        {
            if (_currentlyHovered != null && _currentlyHovered.Equals(value)) return;

            if (_currentlyHovered != null)
            {
                _nonInteractableReticle.SetActive(false);
                _currentlyHovered.UnHovered();
            }
            
            _currentlyHovered = value;
            if (_currentlyHovered != null)
            {
                _nonInteractableReticle.SetActive(true);
                _currentlyHovered.Hovered();
            }
        }
    }
    
    private Rigidbody _heldRigidbody;
    private Holdable _currentlyHeld;

    private Holdable CurrentlyHeld
    {
        get => _currentlyHeld;
        set
        {
            if (_currentlyHeld != null && _currentlyHeld.Equals(value)) return;
            
            if (_currentlyHeld != null)
                _currentlyHeld.Dropped();
            
            _currentlyHeld = value;
            
            if (_currentlyHeld != null)
            {
                _heldRigidbody = _currentlyHeld.gameObject.GetComponent<Rigidbody>();
                _currentlyHeld.Held();
            }
            
            CurrentlyHovered = null;
        }
    }

    private void Start()
    {
        InputRepository.Instance.OnFire += Fire;
        InputRepository.Instance.OnAim += Aim;
    }

    private void Aim()
    {
        if (CurrentlyHeld != null)
            Drop();
    }

    private void Fire()
    {
        if (CurrentlyHovered != null)
            CurrentlyHovered.Interact();
    }

    void FixedUpdate()
    {
        CheckInteractable();
        

        /*if (CurrentlyHeld != null)
        {
            CurrentlyHeld.transform.position = Vector3.Lerp(
                CurrentlyHeld.transform.position,
                _holdPoint.position,
                1f - Mathf.Exp(-lerpSpeed * Time.deltaTime) // Exponential smoothing
            );
        }*/
    }

    public void Drop()
    {
        if (CurrentlyHeld == null) return;
        
        CurrentlyHeld.transform.SetParent(_heldPreviousParent);
        _heldPreviousParent = null;
        
        _heldRigidbody.useGravity = true;
        
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, Camera.main.nearClipPlane));
        Vector3 direction = (worldPosition - Camera.main.transform.position).normalized;
        
        _heldRigidbody.AddForce(direction * _throwForce * _heldRigidbody.mass);
        
        CurrentlyHeld = null;
    }

    public void PickUp(Holdable holdable)
    {
        CurrentlyHeld = holdable;
        CurrentlyHovered = null;
        
        _heldRigidbody.useGravity = false;

        _heldPreviousParent = CurrentlyHeld.transform.parent;

        CurrentlyHeld.transform.SetParent(_holdPoint);
        CurrentlyHeld.transform.localPosition = Vector3.zero;
        CurrentlyHeld.transform.localRotation = Quaternion.identity;
    }

    void CheckInteractable()
    {
        if (CurrentlyHeld != null) return;
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxInteractDistance, interactableLayer))
        {
            var interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (!interactable)
            {
                CurrentlyHovered = null;
                return;
            }

            if (!interactable.CanInteract)
            {
                CurrentlyHovered = null;
                return;
            }
            
            CurrentlyHovered = interactable;
        }
        else
        {
            CurrentlyHovered = null;
        }
    }
}
