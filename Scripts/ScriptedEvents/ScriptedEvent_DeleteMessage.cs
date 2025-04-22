using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_DeleteMessage : ScriptedEvent
{
    [SerializeField] private DeleteSide deleteSide = DeleteSide.Both;
    [SerializeField] private bool waitForPlayerEntry = true;
    
    public enum DeleteSide
    {
        One,
        Two,
        Both
    }

    protected override void OnStart()
    {
        if (waitForPlayerEntry) return;

        switch (deleteSide)
        {
            case DeleteSide.Both:
                MainWallCanvas.Instance.RemoveTextSide1();
                MainWallCanvas.Instance.RemoveTextSide2();
                break;
            case DeleteSide.One:
                MainWallCanvas.Instance.RemoveTextSide1();
                break;
            case DeleteSide.Two:
                MainWallCanvas.Instance.RemoveTextSide2();
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

    private bool side1Entered = false;
    protected override void OnEnteredSide1()
    {
        if (!waitForPlayerEntry || side1Entered) return;

        side1Entered = true;
        MainWallCanvas.Instance.RemoveTextSide2();
        
        if (deleteSide == DeleteSide.Two) Complete();
        else if (deleteSide == DeleteSide.Both && side2Entered) Complete();
    }

    private bool side2Entered = false;
    protected override void OnEnteredSide2()
    {
        if (!waitForPlayerEntry || side2Entered) return;
        
        side2Entered = true;
        
        MainWallCanvas.Instance.RemoveTextSide1();
        
        if (deleteSide == DeleteSide.One) Complete();
        else if (deleteSide == DeleteSide.Both && side1Entered) Complete();
    }
}
