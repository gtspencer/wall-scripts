using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class ScriptedEvent_CompleteWhenUnsighted : ScriptedEvent
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private LayerMask _hitLayer;

    public UnityEvent OnUnseen = new UnityEvent();
    
    private Camera _cam;
    private Transform _player;
    
    protected override void OnStart()
    {
        _cam = Camera.main;
        _player = PlayerInfo.Instance.Player;
    }

    protected override void OnUpdate()
    {
        bool canSpawn = true;
        foreach (var point in spawnPoints)
        {
            Vector3 viewportPoint = _cam.WorldToViewportPoint(point.position);
            
            bool isInView = viewportPoint.x > 0 && viewportPoint.x < 1 &&
                            viewportPoint.y > 0 && viewportPoint.y < 1 &&
                            viewportPoint.z > 0;

            if (isInView)
            {
                // if in camera frustrum, check if occluded
                if (!Physics.Raycast(_player.position, (point.position - _player.position).normalized,
                        Vector3.Distance(_player.position, point.position), _hitLayer))
                {
                    canSpawn = false;
                    break;
                }
            }
        }

        if (canSpawn) Spawn();
    }
    
    private void Spawn()
    {
        OnUnseen.Invoke();
        Complete();
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
