using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEvent_Introduction : ScriptedEvent
{
    protected override void OnStart()
    {
        MainWallCanvas.Instance.WriteImmediateSide1("Hi");
    }

    protected override void OnUpdate()
    {
        
    }
    
    protected override void OnEnd()
    {
        
    }

    private int side1EntranceCount = 0;
    protected override void OnEnteredSide1()
    {
        side1EntranceCount++;

        string message = "";

        switch (side1EntranceCount)
        {
            case 1:
                message = "I'm Wall";
                break;
            case 2:
                message = "I know what you're thinking";
                break;
            case 3:
                message = "Despite being more suited to that name than any other sentient being...";
                break;
            case 4:
                Complete();
                break;
        }
        
        MainWallCanvas.Instance.WriteImmediateSide2(message);
    }
    
    private int side2EntranceCount = 0;
    protected override void OnEnteredSide2()
    {
        side2EntranceCount++;

        string message = "";

        switch (side2EntranceCount)
        {
            case 1:
                message = "You're personal upright companion";
                break;
            case 2:
                message = "And for legal purposes, I can't be called Wall E";
                break;
        }
        
        MainWallCanvas.Instance.WriteImmediateSide1(message);
    }
}
