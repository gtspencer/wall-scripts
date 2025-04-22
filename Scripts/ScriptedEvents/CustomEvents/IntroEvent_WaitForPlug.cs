using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEvent_WaitForPlug : ScriptedEvent
{
    [SerializeField] private Outlet outlet;
    protected override void OnStart()
    {
        outlet.OnPluggedIn.AddListener(OnPluggedIn);
    }

    private void OnPluggedIn()
    {
        outlet.OnPluggedIn.RemoveListener(OnPluggedIn);
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
