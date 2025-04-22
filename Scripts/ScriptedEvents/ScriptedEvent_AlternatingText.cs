using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptedEvent_AlternatingText : ScriptedEvent
{
    // needed in case a previous step is a "wait" step, and we don't trigger OnEnteredSide when we complete the previous wait step
    [SerializeField] private string initialMessage;
    [SerializeField] private SideTriggers.Side initialSideForMessage;
    
    [SerializeField]
    private List<string> side1Messages = new List<string>();
    
    [SerializeField]
    protected List<string> side2Messages = new List<string>();

    private List<float> _timeSpendOnSide = new List<float>();

    private float _timer;
    private float _maxTimeForSmoothBrains = 1f;

    protected override void OnStart()
    {
        if (string.IsNullOrEmpty(initialMessage)) return;

        _timer = 0;

        switch (initialSideForMessage)
        {
            case SideTriggers.Side.Side1:
                MainWallCanvas.Instance.WriteImmediateSide1(FormatString(initialMessage));
                break;
            case SideTriggers.Side.Side2:
                MainWallCanvas.Instance.WriteImmediateSide2(FormatString(initialMessage));
                break;
        }
        
        CheckIfCompleted();
    }

    private int side1Count = -1;
    protected override void OnEnteredSide1()
    {
        _timeSpendOnSide.Add(_timer);
        _timer = 0f;
        
        side1Count++;

        var message = "";
        if (side1Count < side2Messages.Count)
            message = side2Messages[side1Count];
        
        MainWallCanvas.Instance.WriteImmediateSide2(FormatString(message));
        
        CheckIfCompleted();
    }

    private int side2Count = -1;
    protected override void OnEnteredSide2()
    {
        _timeSpendOnSide.Add(_timer);
        _timer = 0f;
        
        side2Count++;

        var message = "";
        if (side2Count < side1Messages.Count)
            message = side1Messages[side2Count];
        
        MainWallCanvas.Instance.WriteImmediateSide1(FormatString(message));
        
        CheckIfCompleted();
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
            
            // TODO make sure this doesn't fuck with any other events
            if (side1Messages.Count <= 0 && side2Messages.Count <= 0) Complete();
        }
        
    }
    
    protected override void OnUpdate()
    {
        _timer += Time.deltaTime;
    }

    protected override void OnEnd()
    {
        if (AchievementManager.SimpleMindedAchieved || _timeSpendOnSide.Count < 3) return;

        var avg = _timeSpendOnSide.Average();
        
        if (avg < _maxTimeForSmoothBrains)
            AchievementManager.AchievementEventRepository.OnSimpleMinded.Invoke();
    }
}
