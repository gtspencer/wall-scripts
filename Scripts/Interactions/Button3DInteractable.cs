using System.Collections;
using System.Collections.Generic;
using Interactions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Button3DInteractable : Interactable
{
    public UnityEvent OnButtonPressed = new UnityEvent();
    [SerializeField]
    private float _pressedValue = -70.54f;

    [SerializeField] private bool _highlightOnPress;

    [SerializeField] private bool selectable;
    [SerializeField] private Material selectedMaterial;

    [SerializeField] private bool _onlyOnePress;

    private AudioClip _buttonClip;

    private bool _selected;

    private Renderer _renderer;
    private Material _initialMaterial;

    private Vector3 _unPressedPosition;
    private LTDescr _lt;

    [SerializeField] private bool _dontPlaySound;
    
    protected override void OnStart()
    {
        _renderer = gameObject.GetComponent<Renderer>();

        _buttonClip = Resources.Load<AudioClip>("Audio/SFX/button");

        _initialMaterial = _renderer.material;

        _unPressedPosition = transform.position;
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
        if (_onlyOnePress && _selected) return;

        if (_lt != null)
            LeanTween.cancel(_lt.id);
        transform.position = _unPressedPosition;
        
        var duration = 1f;
        _lt = LeanTween.moveLocalZ(this.gameObject, _pressedValue, duration).setEasePunch();

        if (_highlightOnPress)
            StartCoroutine(HighlightThenDont(duration));
        
        if (selectable)
            SetSelected(!_selected);

        if (!_dontPlaySound)
            AudioManager.Instance.PlayOneShotSFXAudio(_buttonClip, transform.position);
        
        OnButtonPressed.Invoke();
    }

    private IEnumerator HighlightThenDont(float duration)
    {
        SetSelected(true);

        yield return new WaitForSeconds(duration / 3);

        SetSelected(false);
    }

    protected override void OnHovered()
    {
        
    }

    protected override void OnUnHovered()
    {
        
    }
}
