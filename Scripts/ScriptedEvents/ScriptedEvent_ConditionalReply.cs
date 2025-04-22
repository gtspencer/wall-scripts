using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_ConditionalReply : ScriptedEvent
{
    [SerializeField] private PlayerChoiceKey questionKey;
    [SerializeField] private SideTriggers.Side sideToDisplay = SideTriggers.Side.Side1;
    [SerializeField] private string replyToOption1;
    [SerializeField] private string replyToOption2;

    [SerializeField] private bool waitForBothSidesClear;
    protected override void OnStart()
    {
        var message = PlayerInfo.Instance.GetPlayerChoice(questionKey).DecisionSide == SideTriggers.Side.Side1
            ? replyToOption1
            : replyToOption2;

        switch (sideToDisplay)
        {
            case SideTriggers.Side.Side1:
                MainWallCanvas.Instance.WriteImmediateSide1(message);
                break;
            case SideTriggers.Side.Side2:
                MainWallCanvas.Instance.WriteImmediateSide2(message);
                break;
        }
        
        if (!waitForBothSidesClear) Complete();
    }

    protected override void OnUpdate()
    {
    }

    protected override void OnEnd()
    {
    }

    protected override void OnEnteredSide1()
    {
        if (waitForBothSidesClear) return;

        if (sideToDisplay == SideTriggers.Side.Side1) return;
        
        MainWallCanvas.Instance.RemoveTextSide2();
        Complete();
    }

    protected override void OnEnteredSide2()
    {
        if (waitForBothSidesClear) return;
        
        if (sideToDisplay == SideTriggers.Side.Side2) return;
        
        MainWallCanvas.Instance.RemoveTextSide1();
        Complete();
    }
}
