using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_GlitchWawallText : ScriptedEvent
{
    [SerializeField] private SideTriggers.Side initialSideForMessage;

    [SerializeField] private Texture2D _startingImage;
    
    [SerializeField]
    private List<Texture2D> glitchImages = new List<Texture2D>();

    [SerializeField] private float _minRegularImageTime = 0.5f;
    [SerializeField] private float _maxRegularImageTime = 2f;
    
    [SerializeField] private float _minGlitchImageTime = 0.5f;
    [SerializeField] private float _maxGlitchImageTime = 2f;
    
    private float _timeInCurrentState;
    private float _maxTimeInState;
    private bool _isGlitched;

    protected override void OnStart()
    {
        WawallController.Instance.SetManualGlitch(true);
        
        if (_startingImage == null) return;
        
        switch (initialSideForMessage)
        {
            case SideTriggers.Side.Side1:
                WawallController.Instance.SetSide1Image(_startingImage);
                break;
            case SideTriggers.Side.Side2:
                WawallController.Instance.SetSide2Image(_startingImage);
                break;
        }
    }
    
    protected override void OnEnteredSide1()
    {
        if (initialSideForMessage == SideTriggers.Side.Side2) Complete();
    }
    
    protected override void OnEnteredSide2()
    {
        if (initialSideForMessage == SideTriggers.Side.Side1) Complete();
    }
    
    protected override void OnUpdate()
    {
        _timeInCurrentState += Time.deltaTime;

        if (_timeInCurrentState < _maxTimeInState) return;

        StartCoroutine(GlitchShader());
        
        if (_isGlitched)
        {
            switch (initialSideForMessage)
            {
                case SideTriggers.Side.Side1:
                    WawallController.Instance.SetSide1Image(glitchImages[UnityEngine.Random.Range(0, glitchImages.Count - 1)]);
                    break;
                case SideTriggers.Side.Side2:
                    WawallController.Instance.SetSide2Image(glitchImages[UnityEngine.Random.Range(0, glitchImages.Count - 1)]);
                    break;
            }
            
            _maxTimeInState = UnityEngine.Random.Range(_minRegularImageTime, _maxRegularImageTime);
            _isGlitched = false;
        }
        else
        {
            switch (initialSideForMessage)
            {
                case SideTriggers.Side.Side1:
                    WawallController.Instance.SetSide1Image(_startingImage);
                    break;
                case SideTriggers.Side.Side2:
                    WawallController.Instance.SetSide2Image(_startingImage);
                    break;
            }
            
            _maxTimeInState = UnityEngine.Random.Range(_minGlitchImageTime, _maxGlitchImageTime);
            _isGlitched = true;
        }
        
        _timeInCurrentState = 0;
    }

    private IEnumerator GlitchShader()
    {
        WawallController.Instance.Glitch();
        yield return new WaitForSeconds(0.25f);
        WawallController.Instance.Unglitch();
    }

    protected override void OnEnd()
    {
        WawallController.Instance.Unglitch();
        WawallController.Instance.SetManualGlitch(false);
        WawallController.Instance.SetDefaultTextures();
    }
}
