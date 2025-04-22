using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;

public class TeleportPlayer : ScriptedEvent
{
    [SerializeField] private Transform player;
    [SerializeField] private bool teleportToInitialLocation;
    [SerializeField] private Transform playerTeleportLocation;

    protected override void OnStart()
    {
        if (teleportToInitialLocation)
            TeleportController.Instance.TeleportToStart();
        else
            TeleportController.Instance.TeleportToPoint(playerTeleportLocation.position);
        
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
