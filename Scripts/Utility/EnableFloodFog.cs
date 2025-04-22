using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFloodFog : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private Transform _waterLevel;
    [SerializeField] private GameObject _bubbles;

    [SerializeField]
    private AudioSource _floodSource;
    [SerializeField]
    private AudioSource _underwaterSource;

    [SerializeField] private AudioSource _drownSource;

    private bool _enabled;

    private void Update()
    {
        if (_cameraRoot.position.y < _waterLevel.position.y)
        {
            if (_enabled) return;

            _enabled = true;
            RenderSettings.fogColor = _color;
            RenderSettings.fog = true;
        }
        else
        {
            if (!_enabled) return;

            _enabled = false;
            RenderSettings.fog = false;
        }
    }

    public void SetBubblesActive()
    {
        _bubbles.SetActive(true);
    }

    public void RemoveEffects()
    {
        RenderSettings.fog = false;
        Destroy(_bubbles);
    }

    [SerializeField] private float _footstepFadeDuration;
    [SerializeField] private AudioSource _footstepSource;

    [Header("Flood SFX")]
    [SerializeField] private float _fadeInFloodVolume;
    [SerializeField] private float _fadeInFloodDuration;
    [SerializeField] private float _fadeOutFloodDuration;

    [Header("Underwater SFX")] [SerializeField]
    private float _fadeInUnderwaterVolume;
    [SerializeField]
    private float _fadeInUnderwaterDuration;
    [SerializeField]
    private float _fadeOutUnderwaterDuration;

    [Header("Drown SFX")] [SerializeField] private float _fadeOutDrownDuration;

    public void FadeFloodSFX()
    {
        StartCoroutine(FadeFloodSFX_Coroutine());
    }

    public void PlayAndFadeDrownSFX()
    {
        _drownSource.Play();

        StartCoroutine(FadeSound_Coroutine(_drownSource, _drownSource.volume, 0, _fadeOutDrownDuration));
    }
    
    private IEnumerator FadeFloodSFX_Coroutine()
    {
        _floodSource.Play();
        yield return FadeSound_Coroutine(_floodSource, 0f, _fadeInFloodVolume, _fadeInFloodDuration);
        yield return FadeSound_Coroutine(_floodSource, _floodSource.volume, 0f, _fadeOutFloodDuration);
    }

    public void FadeInUnderwaterSFX()
    {
        _underwaterSource.Play();
        StartCoroutine(FadeSound_Coroutine(_underwaterSource, 0, _fadeInUnderwaterVolume, _fadeInUnderwaterDuration));
    }

    public void FadeOutUnderwaterSFX()
    {
        StartCoroutine(FadeSound_Coroutine(_underwaterSource, _underwaterSource.volume, 0, _fadeOutUnderwaterDuration));
    }

    public void FadeOutFootsteps()
    {
        StartCoroutine(FadeSound_Coroutine(_footstepSource, _footstepSource.volume, 0, _footstepFadeDuration));
    }

    public void ReenableFootsteps()
    {
        _footstepSource.volume = 1f;
    }

    private IEnumerator FadeSound_Coroutine(AudioSource source, float initialVolume, float targetVolume, float duration)
    {
        source.volume = initialVolume;

        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float value = elapsed / duration;

            source.volume = Mathf.Lerp(initialVolume, targetVolume, value);
        
            yield return null;
        }
        
        source.volume = targetVolume;
        
        if (targetVolume == 0) source.Stop();
    }
}
