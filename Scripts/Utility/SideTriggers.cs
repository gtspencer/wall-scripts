using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideTriggers : MonoBehaviour
{
    public static SideTriggers Instance;

    public Side CurrentSide { get; private set; }

    public Action OnPlayerEnteredSide1;
    public Action OnPlayerExitedSide1;
    
    public Action OnPlayerEnteredSide2;
    public Action OnPlayerExitedSide2;

    public enum Side
    {
        Side1,
        Side2
    }

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    public void PlayerEnteredSide1()
    {
        OnPlayerExitedSide2?.Invoke();
        
        OnPlayerEnteredSide1?.Invoke();

        CurrentSide = Side.Side1;
    }

    public void PlayerEnteredSide2()
    {
        OnPlayerExitedSide1?.Invoke();
        
        OnPlayerEnteredSide2?.Invoke();

        CurrentSide = Side.Side2;
    }
}
