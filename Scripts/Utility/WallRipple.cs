using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class WallRipple : MonoBehaviour
{
    private Material _targetMaterial;
    [SerializeField] private float _targetFrequency = 1.0f;
    [SerializeField] private float _targetAmplitude = 1.0f;
    [SerializeField] private float _lerpDuration = 2.0f;

    private Coroutine _ampCoroutine;
    private Coroutine _freqCoroutine;
    
    private static readonly int FrequencyID = Shader.PropertyToID("_Frequency");
    private static readonly int AmplitudeID = Shader.PropertyToID("_Amplitude");

    private void Start()
    {
        _targetMaterial = GetComponent<Renderer>().material;
    }

    [Button]
    public void LerpFrequency()
    {
        if (_freqCoroutine != null)
            StopCoroutine(_freqCoroutine);

        _freqCoroutine = StartCoroutine(LerpFrequency_Coroutine());
    }
    
    [Button]
    public void LerpAmplitude()
    {
        if (_ampCoroutine != null)
            StopCoroutine(_ampCoroutine);

        _ampCoroutine = StartCoroutine(LerpAmplitude_Coroutine());
    }

    private IEnumerator LerpAmplitude_Coroutine()
    {
        float elapsed = 0f;

        var startingAmp = _targetMaterial.GetFloat(AmplitudeID);

        while (elapsed < _lerpDuration)
        {
            float t = elapsed / _lerpDuration;
            float currentAmp = Mathf.Lerp(startingAmp, _targetAmplitude, t);

            _targetMaterial.SetFloat(AmplitudeID, currentAmp);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator LerpFrequency_Coroutine()
    {
        float elapsed = 0f;

        var startingFreq = _targetMaterial.GetFloat(FrequencyID);

        while (elapsed < _lerpDuration)
        {
            float t = elapsed / _lerpDuration;
            float currentFreq = Mathf.Lerp(startingFreq, _targetFrequency, t);

            _targetMaterial.SetFloat(FrequencyID, currentFreq);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
