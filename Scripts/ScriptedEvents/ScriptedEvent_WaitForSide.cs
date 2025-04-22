using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_WaitForSide : ScriptedEvent
{
    [SerializeField] private SideTriggers.Side wantedSide = SideTriggers.Side.Side1;
    protected override void OnStart()
    {
        WaitForSideEntrance(wantedSide, Complete);
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
