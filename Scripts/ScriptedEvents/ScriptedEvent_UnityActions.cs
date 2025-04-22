using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptedEvent_UnityActions : ScriptedEvent
{
    public UnityEvent OnStartEvent = new UnityEvent();
    protected override void OnStart()
    {
        OnStartEvent.Invoke();
        Complete();
    }

    public UnityEvent OnUpdateEvent = new UnityEvent();
    protected override void OnUpdate()
    {
        OnUpdateEvent.Invoke();
    }

    public UnityEvent OnEndEvent = new UnityEvent();
    protected override void OnEnd()
    {
        OnEndEvent.Invoke();
    }

    public UnityEvent OnEnteredSide1Event = new UnityEvent();
    protected override void OnEnteredSide1()
    {
        OnEnteredSide1Event.Invoke();
    }

    public UnityEvent OnEnteredSide2Event = new UnityEvent();
    protected override void OnEnteredSide2()
    {
        OnEnteredSide2Event.Invoke();
    }
}
