using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_SpawnObject : ScriptedEvent
{
    [SerializeField] private List<GameObject> objectsToSetActive = new List<GameObject>();
    [SerializeField] private List<GameObject> objectsToSetInactive = new List<GameObject>();
    [SerializeField] private List<GameObject> objectsToDestroy = new List<GameObject>();
    
    protected override void OnStart()
    {
        foreach (var obj in objectsToSetActive)
        {
            obj.SetActive(true);
        }
        
        foreach (var obj in objectsToSetInactive)
        {
            obj.SetActive(false);
        }
        
        foreach (var obj in objectsToDestroy)
        {
            Destroy(obj);
        }

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
