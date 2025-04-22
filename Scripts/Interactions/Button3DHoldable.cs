using System;
using System.Collections;
using Interactions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Button3DHoldable : Interactable
{
    [SerializeField]
    private float _pressedValue = -70.54f;
    
    [SerializeField] private Material selectedMaterial;

    private Vector3 _startPosition;

    private AudioClip _buttonDownClip;
    private AudioClip _buttonUpClip;

    private bool _selected;

    private Renderer _renderer;
    private Material _initialMaterial;

    public UnityEvent OnDown = new UnityEvent();
    public UnityEvent OnUp = new UnityEvent();
    
    protected override void OnStart()
    {
        _renderer = gameObject.GetComponent<Renderer>();

        _buttonDownClip = Resources.Load<AudioClip>("Audio/SFX/ButtonDown");
        _buttonUpClip = Resources.Load<AudioClip>("Audio/SFX/ButtonUp");

        _initialMaterial = _renderer.material;

        _startPosition = transform.localPosition;
    }

    public void SetInitialValue(bool selected)
    {
        SetSelected(selected);
    }

    private void SetSelected(bool selected)
    {
        _selected = selected;

        _renderer.material = _selected ? selectedMaterial : _initialMaterial;
    }

    [Button]
    protected override void OnInteracted()
    {

    }

    private bool _held;
    private LTDescr _lt;
    
    protected override void OnHovered()
    {
        InputRepository.Instance.OnFireStarted += Clicked;
        InputRepository.Instance.OnFireStopped += Unclicked;
    }
    
    private void Clicked()
    {
        SetSelected(true);
        _held = true;
        
        var duration = .5f;
        if (_lt != null)
            LeanTween.cancel(_lt.id);
        _lt = LeanTween.moveLocalZ(this.gameObject, _pressedValue, duration).setEaseOutElastic();

        AudioManager.Instance.PlayOneShotSFXAudio(_buttonDownClip, transform.position);
        
        OnDown.Invoke();
    }

    private void Unclicked()
    {
        ResetClicked();
        
        AudioManager.Instance.PlayOneShotSFXAudio(_buttonUpClip, transform.position);
    }

    protected override void OnUnHovered()
    {
        InputRepository.Instance.OnFireStarted -= Clicked;
        InputRepository.Instance.OnFireStopped -= Unclicked;
        
        ResetClicked();
    }

    private void ResetClicked()
    {
        SetSelected(false);
        _held = false;
        
        var duration = .5f;
        if (_lt != null)
            LeanTween.cancel(_lt.id);
        _lt = LeanTween.moveLocalZ(this.gameObject, _startPosition.z, duration).setEaseOutElastic();
        
        OnUp.Invoke();
    }

    private void Update()
    {
        // if ()
    }
}
