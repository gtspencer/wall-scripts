using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent_AlternatingWawallText : ScriptedEvent
{
    // needed in case a previous step is a "wait" step, and we don't trigger OnEnteredSide when we complete the previous wait step
    [SerializeField] private Texture2D initialMessage;
    [SerializeField] private SideTriggers.Side initialSideForMessage;
    
    [SerializeField]
    private List<Texture2D> side1Messages = new List<Texture2D>();
    
    [SerializeField]
    protected List<Texture2D> side2Messages = new List<Texture2D>();

    protected override void OnStart()
    {
        if (initialMessage == null) return;

        switch (initialSideForMessage)
        {
            case SideTriggers.Side.Side1:
                WawallController.Instance.SetSide1Image(initialMessage);
                break;
            case SideTriggers.Side.Side2:
                WawallController.Instance.SetSide2Image(initialMessage);
                break;
        }
        
        CheckIfCompleted();
    }

    private int side1Count = -1;
    protected override void OnEnteredSide1()
    {
        side1Count++;

        Texture2D texture = null;
        if (side1Count < side2Messages.Count)
            texture = side2Messages[side1Count];
        
        CheckIfCompleted();
        
        WawallController.Instance.SetSide2Image(texture);
    }

    private int side2Count = -1;
    protected override void OnEnteredSide2()
    {
        side2Count++;

        Texture2D texture = null;
        if (side2Count < side1Messages.Count)
            texture = side1Messages[side2Count];
        
        CheckIfCompleted();
        
        WawallController.Instance.SetSide1Image(texture);
    }

    [SerializeField] private bool waitForBothSidesClear = false;
    private void CheckIfCompleted()
    {
        if (waitForBothSidesClear)
        {
            if (side2Count < side1Messages.Count) return;
            
            if (side1Count < side2Messages.Count) return;

            Complete();
        }
        else
        {
            if (side2Count >= side1Messages.Count || side1Count >= side2Messages.Count) Complete(); 
        }
        
    }
    
    protected override void OnUpdate()
    {

    }

    protected override void OnEnd()
    {
        // WawallController.Instance.SetDefaultTextures();
    }
}
