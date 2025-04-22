using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEvent_PlayerName : ScriptedEvent
{
    protected override void OnStart()
    {
        var choice = PlayerInfo.Instance.GetPlayerChoice(PlayerChoiceKey.Name);
        PlayerInfo.Instance.PlayerName = choice.Choice == "Yes" ? "Carl" : "Not Carl";
        
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
