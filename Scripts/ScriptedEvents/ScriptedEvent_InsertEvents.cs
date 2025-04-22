using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptedEvent_InsertEvents : ScriptedEvent
{
    [SerializeField] private PlayerChoiceKey questionKey;
    [SerializeField] private Transform option1EventObject;
    [SerializeField] private Transform option2EventObject;

    private List<ScriptedEvent> option1Events;
    private List<ScriptedEvent> option2Events;

    private void Start()
    {
        option1Events = option1EventObject.GetComponentsInChildren<ScriptedEvent>().ToList();
        option2Events = option2EventObject.GetComponentsInChildren<ScriptedEvent>().ToList();
    }

    protected override void OnStart()
    {
        switch (PlayerInfo.Instance.GetPlayerChoice(questionKey).DecisionSide)
        {
            case SideTriggers.Side.Side1:
                ScriptedEventController.Instance.InsertEventsAfterCurrent(option1Events);
                break;
            case SideTriggers.Side.Side2:
                ScriptedEventController.Instance.InsertEventsAfterCurrent(option2Events);
                break;
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
