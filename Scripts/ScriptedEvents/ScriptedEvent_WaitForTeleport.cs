using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_WaitForTeleport : ScriptedEvent
{
    protected override void OnStart()
    {
        PortalController.Instance.OnPlayerTeleportFromEntrance += Teleported;
    }

    private void Teleported()
    {
        PortalController.Instance.OnPlayerTeleportFromEntrance -= Teleported;
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
