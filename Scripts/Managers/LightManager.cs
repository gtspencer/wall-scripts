using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    
    [SerializeField] private Light[] _lights;
    
    private float[] _initialIntensities;
    private Color[] _startingColors;

    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);

        Instance = this;
    }

    private void Start()
    {
        _initialIntensities = new float[_lights.Length];
        _startingColors = new Color[_lights.Length];

        for (int i = 0; i < _lights.Length; i++)
        {
            _initialIntensities[i] = _lights[i].intensity;
            _startingColors[i] = _lights[i].color;
        }
    }

    public void IntensityOverTime(float targetIntensity, float duration)
    {
        StartCoroutine(IntensityOverTime_Coroutine(targetIntensity, duration));
    }

    private IEnumerator IntensityOverTime_Coroutine(float targetIntensity, float duration)
    {
        var startingIntensities = new float[_lights.Length];
        for (int i = 0; i < _lights.Length; i++)
        {
            startingIntensities[i] = _lights[i].intensity;
        }
        
        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            for (int i = 0; i < _lights.Length; i++)
            {
                float t = Mathf.Clamp01(elapsedTime / duration);
                
                _lights[i].intensity = Mathf.Lerp(startingIntensities[i], targetIntensity, t);
            }
            
            yield return null;
        }

        foreach (var light in _lights)
        {
            light.intensity = targetIntensity;
        }
    }

    public void ColorOverTime(Color targetColor, float duration)
    {
        StartCoroutine(ColorOverTime_Coroutine(targetColor, duration));
    }

    private IEnumerator ColorOverTime_Coroutine(Color targetColor, float duration)
    {
        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            for (int i = 0; i < _lights.Length; i++)
            {
                float t = Mathf.Clamp01(elapsedTime / duration);
                
                _lights[i].color = Color.Lerp(_startingColors[i], targetColor, t);
            }
            
            yield return null;
        }

        foreach (var light in _lights)
        {
            light.color = targetColor;
        }
    }

    public void ResetLights()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].intensity = _initialIntensities[i];
            _lights[i].color = _startingColors[i];
        }
    }
}
