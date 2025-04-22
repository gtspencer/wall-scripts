using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using URPGlitch;

public class PostProcessingController : MonoBehaviour
{
    public static PostProcessingController Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    
    [SerializeField]
    private Volume volume;

    // [SerializeField] private UniversalRendererData _renderData;

    private ChromaticAberration _chromaticAberration;
    private Coroutine _chromaticAberrationPulseCoroutine;

    /*private AnalogGlitchRenderFeature _analogGlitchFeature;
    private DigitalGlitchFeature _digitalGlitchFeature;*/
    private AnalogGlitchVolume _analogGlitch;
    private DigitalGlitchVolume _digitalGlitch;

    [Header("Screen Glitch Variables")]
    [SerializeField]
    private float _glitchDuration;

    [SerializeField] private float _scanLineJitter = 0.42f;
    [SerializeField] private float _verticleJump = 0.05f;
    [SerializeField] private float _horizontalShake = 0.3f;
    [SerializeField] private float _colorDrift = 0.5f;
    [SerializeField] private float _digitalIntensity = 1f;

    public struct PulseInput
    {
        public float minIntensity;
        public float maxIntensity;
        public float duration;
    }

    private void Start()
    {
        if (!volume.profile.TryGet(out _chromaticAberration))
        {
            Debug.LogWarning("Failed to grab Chromatic Aberration off global volume");
        }
        
        if (!volume.profile.TryGet(out _analogGlitch))
        {
            Debug.LogWarning("Failed to grab Analog Glitch off global volume");
        }
        
        if (!volume.profile.TryGet(out _digitalGlitch))
        {
            Debug.LogWarning("Failed to grab Digital Glitch off global volume");
        }
        
        /*if (!_renderData.TryGetRendererFeature(out _analogGlitchFeature))
        {
            Debug.LogWarning("Failed to grab Analog Glitch off renderer");
        }

        _analogGlitchFeature.SetActive(false);
        
        if (!_renderData.TryGetRendererFeature(out _digitalGlitchFeature))
        {
            Debug.LogWarning("Failed to grab Digital Glitch off renderer");
        }

        _digitalGlitchFeature.SetActive(false);*/
    }

    public void PulseChromaticAberration()
    {
        StopPulseChromaticAberration();
        
        _chromaticAberrationPulseCoroutine = StartCoroutine(PulseChromaticAberration_Coroutine(.35f, .8f, 4));
    }

    public void StopPulseChromaticAberration()
    {
        SetChromaticAberration(0);
        if (_chromaticAberrationPulseCoroutine == null) return;
        
        StopCoroutine(_chromaticAberrationPulseCoroutine);
        _chromaticAberrationPulseCoroutine = null;
    }

    private IEnumerator PulseChromaticAberration_Coroutine(float minIntensity, float maxIntensity, float duration)
    {
        float halfDuration = duration / 2f;

        while (true)
        {
            yield return LerpChromaticAberration(minIntensity, maxIntensity, halfDuration);
            yield return LerpChromaticAberration(maxIntensity, minIntensity, halfDuration);
        }
    }
    
    private IEnumerator LerpChromaticAberration(float from, float to, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            SetChromaticAberration(Mathf.Lerp(from, to, elapsedTime / duration));
            yield return null;
        }
        SetChromaticAberration(to);
    }
    
    public void SetChromaticAberration(float rawValue)
    {
        if (rawValue == 0)
        {
            _chromaticAberration.intensity.Override(0);
            _chromaticAberration.active = false;
            return;
        }
            
        _chromaticAberration.active = true;
        _chromaticAberration.intensity.Override(rawValue);
    }

    private UniversalRendererData GetCurrentRenderAsset()
    {
        UniversalRenderPipelineAsset data = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (data == null)
            return null;
        
        return data.rendererDataList[0] as UniversalRendererData;
    }

    private AnalogGlitchRenderFeature GetAnalogGlitchFeature()
    {
        var renderData = GetCurrentRenderAsset();

        if (renderData == null)
            return null;
        
        if (!renderData.TryGetRendererFeature(out AnalogGlitchRenderFeature analogGlitchFeature))
        {
            return analogGlitchFeature;
        }

        return null;
    }

    private DigitalGlitchFeature GetDigitalGlitchFeature()
    {
        var renderData = GetCurrentRenderAsset();

        if (renderData == null)
            return null;
        
        if (!renderData.TryGetRendererFeature(out DigitalGlitchFeature digitalGlitchFeature))
        {
            return digitalGlitchFeature;
        }

        return null;
    }

    public void ToggleScreenGlitch(bool enabled)
    {
        var analogGlitch = GetAnalogGlitchFeature();
        if (analogGlitch != null)
        {
            analogGlitch.SetActive(enabled);
        }

        var digitalGlitch = GetDigitalGlitchFeature();
        if (digitalGlitch != null)
            digitalGlitch.SetActive(enabled);

        _analogGlitch.active = enabled;
        _digitalGlitch.active = enabled;
    }

    [Button]
    public void DisableScreenGlitch()
    {
        _analogGlitch.scanLineJitter.Override(0);
        _analogGlitch.verticalJump.Override(0);
        _analogGlitch.horizontalShake.Override(0);
        _analogGlitch.colorDrift.Override(0);
            
        _digitalGlitch.intensity.Override(0);

        ToggleScreenGlitch(false);
    }

    public Action OnScreenGlitchLerpDone = () => { };
    
    [Button]
    public void LerpScreenGlitches()
    {
        ToggleScreenGlitch(true);
        StartCoroutine(LerpScreenGlitches_Coroutine());
    }
    
    private IEnumerator LerpScreenGlitches_Coroutine()
    {
        var elapsed = 0f;
        while (elapsed < _glitchDuration)
        {
            elapsed += Time.deltaTime;
            
            float value = Mathf.Clamp01(elapsed / _glitchDuration);

            _analogGlitch.scanLineJitter.Override(_scanLineJitter * value);
            _analogGlitch.verticalJump.Override(_verticleJump * value);
            _analogGlitch.horizontalShake.Override(_horizontalShake * value);
            _analogGlitch.colorDrift.Override(_colorDrift * value);
            
            _digitalGlitch.intensity.Override(_digitalIntensity * value);
            
            yield return null;
        }
        
        OnScreenGlitchLerpDone.Invoke();
    }

    private void OnDisable()
    {
        StopPulseChromaticAberration();
        DisableScreenGlitch();
    }
}
