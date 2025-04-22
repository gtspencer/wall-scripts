using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEvent_PlayerName_old : ScriptedEvent
{
    private bool doneInitialWaiting;
    protected override void OnStart()
    {
        WaitForSeconds(5f, DoneInitialWaiting);
    }

    private void DoneInitialWaiting()
    {
        WaitForSideEntrance(SideTriggers.Side.Side1, DoneWaitingSide1);
    }

    private void DoneWaitingSide1()
    {
        MainWallCanvas.Instance.WriteImmediateSide2("How rude of me, I haven't even asked anything about you.");
        doneInitialWaiting = true;
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnEnd()
    {
        
    }

    private int side1Count = 0;
    protected override void OnEnteredSide1()
    {
        if (!doneInitialWaiting) return;

        side1Count++;

        var message = "";

        switch (side1Count)
        {
            case 1:
                ToggleLockSideCallbacks(true);
                WaitForSeconds(10f, OnDoneWaitingForName);
                break;
            case 2:
                message = "I also don't have eyes, so I can't even see you.";
                break;
            case 3:
                message = "And I sense that you are one lovely individual";
                break;
            case 4:
                Complete();
                break;
        }
        
        MainWallCanvas.Instance.WriteImmediateSide2(message);
    }

    private int side2Count = 0;
    protected override void OnEnteredSide2()
    {
        if (!doneInitialWaiting) return;

        side2Count++;
        
        var message = "";

        switch (side2Count)
        {
            case 1:
                message = "You already know I'm Wall.  What should I call you?";
                break;
            case 2:
                message = "I do have an omniscient sense of my surroundings, which is basically just fancy eyes.\nSo while I can't see you, I can sense you.";
                break;
            case 3:
                message = "But let's try something, I think we can figure out how to properly communicate.";
                break;
        }
        
        MainWallCanvas.Instance.WriteImmediateSide1(message);
    }

    private void OnDoneWaitingForName()
    {
        WaitForSideEntrance(SideTriggers.Side.Side2, OnDoneWaitingForSide2);
    }

    private void OnDoneWaitingForSide2()
    {
        ToggleLockSideCallbacks(false);

        side1Count = 1;
        MainWallCanvas.Instance.WriteImmediateSide1("I just realized I don't have ears, so I can't hear your name.");
    }
}
