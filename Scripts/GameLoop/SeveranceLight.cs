using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class SeveranceLight : MonoBehaviour
{
    [SerializeField]
    private Renderer side1;
    private Material _side1Mat;
    [SerializeField]
    private Renderer side2;
    private Material _side2Mat;

    [Header("Lights")]
    [SerializeField] private float _lightIntensity;
    [SerializeField] private Light _side1Light1;
    [SerializeField] private Light _side1Light2;
    [SerializeField] private Light _side2Light1;
    [SerializeField] private Light _side2Light2;
    
    [Header("Timing")]
    [SerializeField]
    private float _onDuration = 1f;
    [SerializeField]
    private float _offDuration = 3f;

    private Coroutine _pulseCoroutine;

    private void Start()
    {
        _side1Mat = side1.material;
        _side2Mat = side2.material;
    }

    [Button]
    public void PulseElement()
    {
        if (_pulseCoroutine != null) StopCoroutine(_pulseCoroutine);
        
        _pulseCoroutine = StartCoroutine(PulseElement_Coroutine());
    }
    
    private IEnumerator PulseElement_Coroutine()
    {
        yield return FadeOn();
        yield return FadeOff();
    }

    private IEnumerator FadeOn()
    {
        Color startColor = Color.black;
        Color endColor = Color.white;
        float startEmission = 0f;
        float endEmission = 1f;

        // Enable emission
        _side1Mat.EnableKeyword("_EMISSION");
        _side1Mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        
        _side2Mat.EnableKeyword("_EMISSION");
        _side2Mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        
        float elapsedTime = 0f;
        while (elapsedTime < _onDuration)
        {
            float t = elapsedTime / _onDuration;

            _side1Light1.intensity = t * _lightIntensity;
            _side1Light2.intensity = t * _lightIntensity;
            _side2Light1.intensity = t * _lightIntensity;
            _side2Light2.intensity = t * _lightIntensity;
            
            // Lerp color
            _side1Mat.color = Color.Lerp(startColor, endColor, t);
            _side2Mat.color = Color.Lerp(startColor, endColor, t);
            
            // Lerp emission intensity
            Color emissionColor = Color.Lerp(startColor, endColor, t) * Mathf.Lerp(startEmission, endEmission, t);
            _side1Mat.SetColor("_EmissionColor", emissionColor);
            _side2Mat.SetColor("_EmissionColor", emissionColor);
            
            // Update GI
            DynamicGI.SetEmissive(side1, emissionColor);
            DynamicGI.SetEmissive(side2, emissionColor);
            DynamicGI.UpdateEnvironment();
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        _side1Mat.color = endColor;
        _side1Mat.SetColor("_EmissionColor", endColor * endEmission);
        
        _side2Mat.color = endColor;
        _side2Mat.SetColor("_EmissionColor", endColor * endEmission);
        
        DynamicGI.SetEmissive(side1, endColor * endEmission);
        DynamicGI.SetEmissive(side2, endColor * endEmission);
        DynamicGI.UpdateEnvironment();
    }
    
    private IEnumerator FadeOff()
    {
        Color endColor = Color.black;
        Color startColor = Color.white;
        float endEmission = 0f;
        float startEmission = 1f;
        
        float elapsedTime = 0f;
        while (elapsedTime < _offDuration)
        {
            float t = elapsedTime / _offDuration;

            _side1Light1.intensity = Mathf.Lerp(_lightIntensity, 0, t);
            _side1Light2.intensity = Mathf.Lerp(_lightIntensity, 0, t);
            _side2Light1.intensity = Mathf.Lerp(_lightIntensity, 0, t);
            _side2Light2.intensity = Mathf.Lerp(_lightIntensity, 0, t);
            
            // Lerp color
            _side1Mat.color = Color.Lerp(startColor, endColor, t);
            _side2Mat.color = Color.Lerp(startColor, endColor, t);
            
            // Lerp emission intensity
            Color emissionColor = Color.Lerp(startColor, endColor, t) * Mathf.Lerp(startEmission, endEmission, t);
            _side1Mat.SetColor("_EmissionColor", emissionColor);
            _side2Mat.SetColor("_EmissionColor", emissionColor);
            
            // Update GI
            DynamicGI.SetEmissive(side1, emissionColor);
            DynamicGI.SetEmissive(side2, emissionColor);
            DynamicGI.UpdateEnvironment();
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        _side1Mat.color = endColor;
        _side2Mat.color = endColor;
        
        _side1Mat.DisableKeyword("_EMISSION");
        
        _side2Mat.DisableKeyword("_EMISSION");
        
        _side1Light1.intensity = 0;
        _side1Light2.intensity = 0;
        _side2Light1.intensity = 0;
        _side2Light2.intensity = 0;
        
        DynamicGI.SetEmissive(side1, endColor * endEmission);
        DynamicGI.SetEmissive(side2, endColor * endEmission);
        DynamicGI.UpdateEnvironment();
    }
}
