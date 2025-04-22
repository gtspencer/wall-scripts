using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

public class Butterfly : MonoBehaviour
{
    private Material butterflyMaterial;

    [SerializeField] private float splineFlyTime = 7f;
    [SerializeField] private float colorFadeTime = 5f;
    [SerializeField] private float trailTime = 2f;
    [SerializeField] private SplineContainer splineContainer;

    [SerializeField] private Transform rigRoot;
    
    private float fadeTimer;
    private bool shouldFade;

    private float startingR;
    private float startingG;
    private float startingB;

    private Renderer _renderer;
    private TrailRenderer trail;

    private Light _light;
    private float startingIntensity;
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    void OnEnable()
    {
        if (splineContainer == null)
        {
            Debug.LogError($"Spline container for butterfly {gameObject.name} is null.  Destroying to save resources");
            Destroy(gameObject);
            return;
        }
        
        _renderer = GetComponentInChildren<Renderer>();
        butterflyMaterial = _renderer.material;

        #region Spline
        var splineAnimate = gameObject.AddComponent<SplineAnimate>();
        splineAnimate.Loop = SplineAnimate.LoopMode.Once;
        splineAnimate.Container = splineContainer;
        splineAnimate.PlayOnAwake = false;
        splineAnimate.Duration = splineFlyTime;
        #endregion
        
        #region Trail Renderer
        trail = rigRoot.gameObject.AddComponent<TrailRenderer>();
        trail.material = butterflyMaterial;
        trail.startWidth = .05f;
        trail.endWidth = .005f;
        trail.time = trailTime;
        trail.shadowCastingMode = ShadowCastingMode.Off;
        trail.emitting = false;
        #endregion
        
        _light = GetComponentInChildren<Light>();

        startingIntensity = _light.intensity;
        
        var butterflyColor = butterflyMaterial.GetColor(EmissionColorProperty);
        startingR = butterflyColor.r;
        startingG = butterflyColor.g;
        startingB = butterflyColor.b;
        
        splineAnimate.Play();
        Fade();
        StartCoroutine(StartTrail());
    }

    private IEnumerator StartTrail()
    {
        yield return new WaitForSeconds(0.1f);
        trail.emitting = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!shouldFade) return;
        
        var butterflyColor = butterflyMaterial.GetColor(EmissionColorProperty);

        var progress = fadeTimer / colorFadeTime;

        var r = startingR * progress;
        var g = startingG * progress;
        var b = startingB * progress;

        butterflyColor.r = r;
        butterflyColor.g = g;
        butterflyColor.b = b;
        
        butterflyMaterial.SetColor(EmissionColorProperty, butterflyColor);

        _light.intensity = startingIntensity * progress;

        fadeTimer -= Time.deltaTime;

        if (fadeTimer <= 0)
        {
            shouldFade = false;
            
            butterflyMaterial.DisableKeyword("_EMISSION");
            _light.intensity = 0;

            StartCoroutine(DestroyAfterSeconds());
        }
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(splineFlyTime - colorFadeTime);
        Destroy(gameObject);
    }

    public void Fade()
    {
        shouldFade = true;
        fadeTimer = colorFadeTime;
    }
}
