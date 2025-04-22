using UnityEngine;

public class Event_WaitForEndButtonPress : ScriptedEvent
{
    public void OnButtonPressed()
    {
        Complete();
    }
    
    protected override void OnStart()
    {
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
