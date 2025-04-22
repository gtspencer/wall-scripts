using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CrowdedPerson : MonoBehaviour
{
    [SerializeField]
    private DecalProjector _faceDecalProjector;
    [SerializeField]
    private DecalProjector _iSeeYouDecalProjector;

    [SerializeField] private float _alphaFadeDuration = .25f;

    private Coroutine fadeToISeeCoroutine;
    private Coroutine fadeToFaceCoroutine;

    private void Start()
    {
        _iSeeYouDecalProjector.fadeFactor = 0f;
    }

    [Button]
    public void ISeeYou()
    {
        StopCoroutines();
        fadeToISeeCoroutine = StartCoroutine(FadeToISeeYou_Coroutine());
    }
    
    [Button]
    public void FadeToFace()
    {
        StopCoroutines();
        fadeToFaceCoroutine = StartCoroutine(FadeToFace_Coroutine());
    }

    private void StopCoroutines()
    {
        if (fadeToISeeCoroutine != null)
        {
            StopCoroutine(fadeToISeeCoroutine);
            fadeToISeeCoroutine = null;
        }

        if (fadeToFaceCoroutine != null)
        {
            StopCoroutine(fadeToFaceCoroutine);
            fadeToFaceCoroutine = null;
        }
    }

    private IEnumerator FadeToISeeYou_Coroutine()
    {
        var elapsed = 0f;
        var fadeTime = _alphaFadeDuration / 2;
        var startLerp = _faceDecalProjector.fadeFactor;
        var endLerp = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            var t = elapsed / fadeTime;

            _faceDecalProjector.fadeFactor = Mathf.Lerp(startLerp, endLerp, t);
            
            // _faceDecalProjector.fadeFactor = t;
            yield return null;
        }

        elapsed = 0f;
        startLerp = _iSeeYouDecalProjector.fadeFactor;
        endLerp = 1f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            var t = elapsed / fadeTime;
            
            _iSeeYouDecalProjector.fadeFactor = Mathf.Lerp(startLerp, endLerp, t);
            
            // _iSeeYouDecalProjector.fadeFactor = t;
            yield return null;
        }
    }
    
    private IEnumerator FadeToFace_Coroutine()
    {
        var elapsed = 0f;
        var fadeTime = _alphaFadeDuration / 2;
        var startLerp = _iSeeYouDecalProjector.fadeFactor;
        var endLerp = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            var t = elapsed / fadeTime;

            _iSeeYouDecalProjector.fadeFactor = Mathf.Lerp(startLerp, endLerp, t);
            
            // _faceDecalProjector.fadeFactor = t;
            yield return null;
        }

        elapsed = 0f;
        startLerp = _faceDecalProjector.fadeFactor;
        endLerp = 1f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            var t = elapsed / fadeTime;
            
            _faceDecalProjector.fadeFactor = Mathf.Lerp(startLerp, endLerp, t);
            
            // _iSeeYouDecalProjector.fadeFactor = t;
            yield return null;
        }
    }
}
