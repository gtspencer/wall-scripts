using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_WaitForSeconds : ScriptedEvent
{
    [SerializeField] private float waitTime;
    protected override void OnStart()
    {
        WaitForSeconds(waitTime, Complete);
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
