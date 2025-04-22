using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersPerspectiveOnWall : ScriptedEvent
{
    [SerializeField] private Material material;
    [SerializeField] private Renderer wallRenderer;
    [SerializeField] private GameObject _playerViewCamera;
    [SerializeField] private bool _setPlayerViewCameraActive;

    protected override void OnStart()
    {
        _playerViewCamera.SetActive(_setPlayerViewCameraActive);
        wallRenderer.material = material;
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
