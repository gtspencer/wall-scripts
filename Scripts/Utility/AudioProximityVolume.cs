using System;
using UnityEngine;

public class AudioProximityVolume : MonoBehaviour
{
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    private float _currentVolumeValue;
    public float CurrentVolumeValue => _currentVolumeValue;
    public bool DontSetVolumeHere;

    private AudioSource _audioSource;
    private Transform _camera;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0f;

        _camera = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        var distance = Vector3.Distance(_camera.position, transform.position);
        
        // no sound
        if (distance > _maxDistance) _currentVolumeValue = 0f;
        // loudest sound
        else if (distance < _minDistance) _currentVolumeValue = _maxVolume;
        else
        {
            var delta = _maxDistance - _minDistance;

            var rawValue = 1 - ((distance - _minDistance) / delta);

            _currentVolumeValue = rawValue * _maxVolume;
        }

        if (!DontSetVolumeHere)
            _audioSource.volume = _currentVolumeValue;
    }
}
