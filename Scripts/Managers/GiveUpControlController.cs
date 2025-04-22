using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public class GiveUpControlController : MonoBehaviour
{
    [SerializeField] private Material _invalidMaterial;
    [SerializeField] private Material _validMaterial;
    [SerializeField]
    private Renderer[] _lights;

    [SerializeField] private Transform _door;
    private Coroutine _doorCoroutine;
    private Vector3 _closedDoorPosition;
    [SerializeField] private Vector3 _openDoorLocalPosition = new Vector3(-2.02999997f, 1.5f, 39.0953751f);
    private AudioSource _doorOpenSource;
    private bool _isDoorOpen;
    
    private int _trueCount;
    private int TrueCount
    {
        get => _trueCount;
        set
        {
            if (value == _trueCount) return;
            
            _trueCount = value;

            switch (_trueCount)
            {
                case 0:
                    foreach (var l in _lights)
                        l.material = _invalidMaterial;
                    break;
                case 1:
                    _lights[0].material = _validMaterial;
                    _lights[1].material = _invalidMaterial;
                    _lights[2].material = _invalidMaterial;
                    break;
                case 2:
                    _lights[0].material = _validMaterial;
                    _lights[1].material = _validMaterial;
                    _lights[2].material = _invalidMaterial;
                    break;
                case 3:
                    _lights[0].material = _validMaterial;
                    _lights[1].material = _validMaterial;
                    _lights[2].material = _validMaterial;
                    break;
            }
            
            ToggleDoor(TrueCount >= 3);
        }
    }

    private void Start()
    {
        foreach (var l in _lights)
            l.material = _invalidMaterial;

        _closedDoorPosition = _door.localPosition;

        _doorOpenSource = GetComponent<AudioSource>();
    }

    public void ResetControls()
    {
        FirstPersonController.Instance.ResetGoneControls();
    }

    private bool _upPressed;
    
    public void OnUp()
    {
        _upPressed = !_upPressed;
        FirstPersonController.Instance.GiveUpForwardControl(_upPressed);
        UpdateState();
    }
    
    private bool _downPressed;
    
    public void OnDown()
    {
        _downPressed = !_downPressed;
        FirstPersonController.Instance.GiveUpBackControl(_downPressed);
        UpdateState();
    }
    
    private bool _leftPressed;
    
    public void OnLeft()
    {
        _leftPressed = !_leftPressed;
        FirstPersonController.Instance.GiveUpLeftControl(_leftPressed);
        UpdateState();
    }
    
    private bool _rightPressed;
    
    public void OnRight()
    {
        _rightPressed = !_rightPressed;
        FirstPersonController.Instance.GiveUpRightControl(_rightPressed);
        UpdateState();
    }

    private void UpdateState()
    {
        TrueCount = (_upPressed ? 1 : 0) + (_downPressed ? 1 : 0) + (_leftPressed ? 1 : 0) + (_rightPressed ? 1 : 0);
    }

    private void ToggleDoor(bool open)
    {
        if (_doorCoroutine != null)
        {
            StopCoroutine(_doorCoroutine);
            _doorCoroutine = null;
        }

        if (open != _isDoorOpen)
        {
            _isDoorOpen = open;
            _doorOpenSource.Stop();
            _doorOpenSource.time = 0;
            _doorOpenSource.Play();
        }
        
        _doorCoroutine = StartCoroutine(ToggleDoor_Coroutine(open));
    }

    private IEnumerator ToggleDoor_Coroutine(bool open)
    {
        var elapsed = 0f;
        var duration = 1f;
        var startPosition = _door.localPosition;
        var endPosition = open ? _openDoorLocalPosition : _closedDoorPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = elapsed / duration;

            _door.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            
            yield return null;
        }
    }
}
