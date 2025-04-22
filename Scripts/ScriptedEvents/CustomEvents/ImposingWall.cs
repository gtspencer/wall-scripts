using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ImposingWall : ScriptedEvent
{
    // cached variables
    private Transform wallMesh;
    private Transform wallController;
    
    [SerializeField]
    private Color targetColor = Color.red;

    [SerializeField]
    private Color _targetLightColor;
    // private float _lightIntensity = 10;
    private float _duration = 10;

    private readonly int _emissionColorName = Shader.PropertyToID("_EmissionColor");

    [SerializeField] private bool _waitTillDoneToComplete = false;
    [SerializeField] private bool _resetWall = false;

    private Vector3 _initialScale;
    private Vector3 _initialPosition;
    private Color _initialColor;

    private void Start()
    {
        wallMesh = World.Instance.WallMesh;
        wallController = World.Instance.WallController;
        
        _initialScale = wallMesh.localScale;
        _initialPosition = wallMesh.position;
        
        var renderer = wallMesh.GetComponent<Renderer>();
        _initialColor = renderer.material.color;
    }

    protected override void OnStart()
    {
        if (_resetWall)
        {
            ResetWall();
            Complete();
            return;
        }
        
        StartCoroutine(StartImposing_Coroutine(2f));
        
        if (!_waitTillDoneToComplete)
            Complete();
    }

    private void ResetWall()
    {
        var renderer = wallMesh.GetComponent<Renderer>();
        renderer.material.color = _initialColor;
        renderer.material.DisableKeyword("_EMISSION");
        
        wallMesh.localScale = _initialScale;
        wallMesh.position = _initialPosition;
    }

    private IEnumerator StartImposing_Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        StartImposing();
    }

    private void StartImposing()
    {
        StartCoroutine(CloseSides());
        StartCoroutine(TurnRed());
        
        LightManager.Instance.ColorOverTime(_targetLightColor, _duration);
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnEnd()
    {
    }

    protected override void OnEnteredSide1()
    {
    }

    protected override void OnEnteredSide2()
    {
    }

    private IEnumerator CloseSides()
    {
        float time = 1f;
        float elapsedTime = 0;
        Vector3 startScale = wallMesh.localScale;
        Vector3 endScale = new Vector3(startScale.x, 20, 20);

        Vector3 startPosition = wallMesh.position;
        Vector3 endPosition = new Vector3(startPosition.x, 10, startPosition.z);

        bool feedbackPlayed = false;
        float screenshakePlayPreDelay = .2f;

        while (elapsedTime < time)
        {
            // Linearly interpolate the scale on the Z-axis over time
            wallMesh.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
            wallMesh.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / time);

            elapsedTime += Time.deltaTime;
            
            // screen shake plays with a little delay and idk the settings to change that
            if (!feedbackPlayed && elapsedTime >= time - screenshakePlayPreDelay)
            {
                feedbackPlayed = true;
                FeedbackManager.Instance.ShakeScreen();
            }
            
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        wallMesh.localScale = endScale;
    }

    private IEnumerator TurnRed()
    {
        float elapsedTime = 0;

        var renderer = wallMesh.GetComponent<Renderer>();
        var material = renderer.material;
        var initialColor = renderer.material.color;
        material.EnableKeyword("_EMISSION");
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        
        while (elapsedTime < _duration)
        {
            // Linearly interpolate the color over time
            var newColor = Color.Lerp(initialColor, targetColor, elapsedTime / _duration);
            material.color = newColor;
            
            float intensity = Mathf.Lerp(0f, 2f, elapsedTime / _duration);

            var emissionColor = targetColor * intensity;
            material.SetColor(_emissionColorName, emissionColor);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is exactly the target color
        renderer.material.color = targetColor;
        material.SetColor(_emissionColorName, targetColor * 2);
        
        Complete();
    }
}
