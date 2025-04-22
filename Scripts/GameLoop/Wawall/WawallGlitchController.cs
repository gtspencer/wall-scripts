using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WawallGlitchController : MonoBehaviour
{
    [SerializeField]
    private Material _side1Material;
    [SerializeField]
    private Material _side1GlitchMaterial;
    
    [SerializeField]
    private Material _side2Material;
    [SerializeField]
    private Material _side2GlitchMaterial;
    
    [SerializeField]
    private MeshRenderer _side1ImageRenderer;
    [SerializeField]
    private MeshRenderer _side2ImageRenderer;
    
    [Header("Glitch settings")]
    [SerializeField]
    private float _minGlitchTime = 0.5f;
    [SerializeField]
    private float _maxGlitchTime = 3.0f;
    [SerializeField]
    private float _minNormalTime = 2.0f;
    [SerializeField]
    private float _maxNormalTime = 5.0f;

    private bool _manualGlitch = false;
    private Coroutine _glitchCoroutine;

    // TODO add glitch sfx
    [SerializeField] private AudioClip _glitch;
    [SerializeField] private AudioSource _source;
    

    private void OnEnable()
    {
        _glitchCoroutine = StartCoroutine(Glitch_Coroutine());
        _source = GetComponentInChildren<AudioSource>();
    }
    
    private IEnumerator Glitch_Coroutine()
    {
        while (true)
        {
            Glitch();
            float glitchDuration = Random.Range(_minGlitchTime, _maxGlitchTime);
            yield return new WaitForSeconds(glitchDuration);

            Unglitch();
            float normalDuration = Random.Range(_minNormalTime, _maxNormalTime);
            yield return new WaitForSeconds(normalDuration);
        }
    }
    
    public void SetSide1Image(Texture2D image)
    {
        _side1Material.mainTexture = image;
        _side1GlitchMaterial.SetTexture("_BaseMap", image);
    }
    
    public void SetSide2Image(Texture2D image)
    {
        _side2Material.mainTexture = image;
        _side2GlitchMaterial.SetTexture("_BaseMap", image);
    }

    public void SetManualGlitch(bool manualGlitch)
    {
        _manualGlitch = manualGlitch;
        Unglitch();

        StopGlitchCoroutine();
        
        if (!_manualGlitch)
        {
            _glitchCoroutine = StartCoroutine(Glitch_Coroutine());
        }
    }

    private void StopGlitchCoroutine()
    {
        if (_glitchCoroutine == null) return;
        
        StopCoroutine(_glitchCoroutine);
        _glitchCoroutine = null;
    }

    [Button]
    public void Glitch()
    {
        _side1ImageRenderer.material = _side1GlitchMaterial;
        _side2ImageRenderer.material = _side2GlitchMaterial;
    }

    [Button]
    public void Unglitch()
    {
        _side1ImageRenderer.material = _side1Material;
        _side2ImageRenderer.material = _side2Material;
    }
}
