using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.VFX;

public class LightningController : MonoBehaviour
{
    [SerializeField] private VisualEffect lightningEffect;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private float _lightUpTime = .2f;
    [SerializeField] private float _lightDownTime = .4f;
    [SerializeField] private float _maxIntensity = 50;
    [SerializeField] private float _lightDelay = .1f;
    [SerializeField] private float _soundDelay = .1f;

    private Light _light;
    
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponentInChildren<Light>();
        _light.intensity = 0;
    }

    [Button]
    public void PlayEffect()
    {
        lightningEffect.Play();
        StartCoroutine(Light());
        StartCoroutine(Sound());
    }

    private IEnumerator Light()
    {
        yield return new WaitForSeconds(_lightDelay);
        
        StartCoroutine(LightErUp());
    }
    
    private IEnumerator Sound()
    {
        yield return new WaitForSeconds(_soundDelay);
        
        AudioManager.Instance.PlayOneShotSFXAudio(sfx, transform.position);
    }

    private IEnumerator LightErUp()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < _lightUpTime)
        {
            _light.intensity = Mathf.Lerp(0, _maxIntensity, elapsedTime / _lightUpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _light.intensity = _maxIntensity;
        elapsedTime = 0f;
        
        while (elapsedTime < _lightDownTime)
        {
            _light.intensity = Mathf.Lerp(_maxIntensity, 0, elapsedTime / _lightDownTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _light.intensity = 0;
    }
}
