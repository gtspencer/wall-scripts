using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PulseVignette : MonoBehaviour
{
    public static PulseVignette Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    
    [SerializeField]
    private Volume volum;
    [SerializeField]
    private RawImage fadeImage;
    [SerializeField]
    private int pulseNumber = 5;
    [SerializeField]
    private float pulseDuration = 0.5f;
    [SerializeField]public float finalVignetteIntensity = 1f;
    private float fadeDuration = 2f;

    private Vignette vignette;
    
    [Button]
    public void StartEffect()
    {
        if (volum.profile.TryGet(out vignette))
        {
            StartCoroutine(PulseVignette_Coroutine());
        }
    }
    
    private IEnumerator PulseVignette_Coroutine()
    {
        float baseIntensity = vignette.intensity.value;
        vignette.intensity.Override(baseIntensity);
        float targetIntensity = finalVignetteIntensity;
        
        for (int i = 0; i < pulseNumber; i++)
        {
            float peak = Mathf.Lerp(baseIntensity, targetIntensity, (i + 1f) / pulseNumber);
            yield return LerpVignette_Coroutine(peak, pulseDuration / 2);
            yield return LerpVignette_Coroutine(peak * 0.8f, pulseDuration / 2);
        }
        
        yield return LerpVignette_Coroutine(finalVignetteIntensity, pulseDuration);
        StartCoroutine(FadeInImage_Coroutine());
    }

    private IEnumerator LerpVignette_Coroutine(float target, float duration)
    {
        float start = vignette.intensity.value;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            vignette.intensity.Override(Mathf.Lerp(start, target, time / duration));
            yield return null;
        }
        vignette.intensity.Override(target);
    }

    private IEnumerator FadeInImage_Coroutine()
    {
        Color color = fadeImage.color;
        float time = 0f;
        
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
    }

    public void ClearEffect()
    {
        Destroy(fadeImage.gameObject);
        vignette.active = false;
    }
}
