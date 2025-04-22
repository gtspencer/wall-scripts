using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ScriptedEvent_DimLights : ScriptedEvent
{
    [SerializeField] private Light[] _lights;
    [SerializeField] private float _duration;
    [SerializeField] private float _intensity;
    [SerializeField] private bool _waitForDurationToComplete = true;

    protected override void OnStart()
    {
        LightManager.Instance.IntensityOverTime(_intensity, _duration);
        
        if (!_waitForDurationToComplete)
            Complete();
        else
            StartCoroutine(WaitForDuration());
    }

    private IEnumerator WaitForDuration()
    {
        yield return new WaitForSeconds(_duration);
        
        Complete();
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
}
