using System.Collections;
using UnityEngine;

public class RoomPathIndicator : MonoBehaviour
{
    [SerializeField] private Material _invalidMaterial;
    [SerializeField] private Material _validMaterial;
    [SerializeField]
    private Renderer _light;

    [SerializeField] private bool _startValid;

    [Header("Door")]
    [SerializeField] private Transform _door;
    private Coroutine _doorCoroutine;
    private Vector3 _closedDoorPosition;
    [SerializeField] private Vector3 _openDoorLocalPosition = new Vector3(-2.02999997f, 1.5f, 39.0953751f);
    [SerializeField] private AudioSource _doorOpenSource;
    private bool _isDoorOpen;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponentInChildren<Renderer>();

        _closedDoorPosition = _door.localPosition;

        if (_startValid) ToggleValid(true, false);
    }

    public void ToggleValid(bool valid, bool playSfx = true)
    {
        _light.material = valid ? _validMaterial : _invalidMaterial;
        ToggleDoor(valid, playSfx);
    }
    
    private void ToggleDoor(bool open, bool playSfx = true)
    {
        if (_doorCoroutine != null)
        {
            StopCoroutine(_doorCoroutine);
            _doorCoroutine = null;
        }

        if (open != _isDoorOpen)
        {
            _isDoorOpen = open;

            if (playSfx)
            {
                _doorOpenSource.Stop();
                _doorOpenSource.time = 0;
                _doorOpenSource.Play();
            }
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
