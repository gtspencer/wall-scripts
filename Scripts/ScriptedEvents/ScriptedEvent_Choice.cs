using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_Choice : ScriptedEvent
{
    [SerializeField] private PlayerChoiceKey questionKey;
    [SerializeField] private string question;
    [SerializeField] private string choice1;
    [SerializeField] private string choice2;

    [SerializeField] private float questionWaitTime = 10f;
    private float counter;
    
    protected override void OnStart()
    {
        MainWallCanvas.Instance.WriteImmediateSide1(questionWaitTime + "\n" + question + "\n" + choice1);
        MainWallCanvas.Instance.WriteImmediateSide2(questionWaitTime + "\n" + question + "\n" + choice2);

        counter = questionWaitTime;
    }

    protected override void OnUpdate()
    {
        counter -= Time.deltaTime;

        var counterInt = (int)counter;
        
        MainWallCanvas.Instance.WriteImmediateSide1(counterInt + "\n" + question + "\n" + choice1);
        MainWallCanvas.Instance.WriteImmediateSide2(counterInt + "\n" + question + "\n" + choice2);
        
        if (counter <= 0) Complete();
    }

    protected override void OnEnd()
    {
        PlayerInfo.Instance.AddPlayerChoice(questionKey, new PlayerChoice(SideTriggers.Instance.CurrentSide == SideTriggers.Side.Side1 ? choice1 : choice2, SideTriggers.Instance.CurrentSide));
        
        MainWallCanvas.Instance.RemoveTextSide1();
        MainWallCanvas.Instance.RemoveTextSide2();
    }

    protected override void OnEnteredSide1()
    {
        
    }

    protected override void OnEnteredSide2()
    {
        
    }
}
