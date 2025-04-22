using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptedEvent : MonoBehaviour
{
    public Action<ScriptedEvent> OnEventComplete;
    private float _timeInEventSec = 0;

    [SerializeField] private bool doNotExecuteStep = false;
    
    protected abstract void OnStart();

    public void StartEvent()
    {
        if (doNotExecuteStep)
        {
            Complete();
            return;
        }
        
        SideTriggers.Instance.OnPlayerEnteredSide1 += EnteredSide1;
        SideTriggers.Instance.OnPlayerEnteredSide2 += EnteredSide2;

        try
        {
            OnStart();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to start event {gameObject.name}.  Logging exception.");
            Debug.LogException(ex);
        }
    }
    
    protected abstract void OnUpdate();
    public void UpdateEvent()
    {
        _timeInEventSec += Time.deltaTime;
        
        try
        {
            OnUpdate();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update event {gameObject.name}.  Logging exception.");
            Debug.LogException(ex);
        }
        
    }

    protected abstract void OnEnd();

    public void EndEvent()
    {
        SideTriggers.Instance.OnPlayerEnteredSide1 -= EnteredSide1;
        SideTriggers.Instance.OnPlayerEnteredSide2 -= EnteredSide2;
        
        try
        {
            OnEnd();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to end event {gameObject.name}.  Logging exception.");
            Debug.LogException(ex);
        }
    }

    protected void Complete()
    {
        OnEventComplete?.Invoke(this);
    }
    
    #region Side Logic

    private bool sideCallbacksLocked;
    protected abstract void OnEnteredSide1();
    private void EnteredSide1()
    {
        if (sideCallbacksLocked) return;
        
        OnEnteredSide1();
    }

    protected abstract void OnEnteredSide2();
    private void EnteredSide2()
    {
        if (sideCallbacksLocked) return;
        
        OnEnteredSide2();
    }
    #endregion
    
    #region Helper Functions
    protected void WaitForSideEntrance(SideTriggers.Side wantedSide, Action sideEnteredCallback)
    {
        if (SideTriggers.Instance.CurrentSide == wantedSide)
        {
            sideEnteredCallback.Invoke();
            return;
        }

        StartCoroutine(WaitForSideEntrance_Coroutine(wantedSide, sideEnteredCallback));
    }
    
    private IEnumerator WaitForSideEntrance_Coroutine(SideTriggers.Side wantedSide, Action sideEnteredCallback)
    {
        while (SideTriggers.Instance.CurrentSide != wantedSide) yield return null;
        
        sideEnteredCallback.Invoke();
    }

    protected void WaitForSeconds(float waitTime, Action doneWaitingCallback)
    {
        StartCoroutine(WaitForSeconds_Coroutine(waitTime, doneWaitingCallback));
    }
    
    private IEnumerator WaitForSeconds_Coroutine(float waitTime, Action doneWaitingCallback)
    {
        yield return new WaitForSeconds(waitTime);
        doneWaitingCallback.Invoke();
    }

    protected void ToggleLockSideCallbacks(bool locked)
    {
        sideCallbacksLocked = locked;
    }

    protected void LockSideCallbacksForSeconds(float lockTime, Action OnUnlock)
    {
        StartCoroutine(LockSideCallbacksForSeconds_Coroutine(lockTime, OnUnlock));
    }

    private IEnumerator LockSideCallbacksForSeconds_Coroutine(float lockTime, Action OnUnlock)
    {
        ToggleLockSideCallbacks(true);
        
        yield return new WaitForSeconds(lockTime);
        
        ToggleLockSideCallbacks(false);
        OnUnlock.Invoke();
    }

    public override string ToString()
    {
        return gameObject.name;
    }

    public float GetTimeInEvent()
    {
        return _timeInEventSec;
    }

    protected string FormatString(string str)
    {
        return str.Replace("{name}", PlayerInfo.Instance.PlayerName).Replace("\\n", "\n");
    }

    public void ToggleShouldExecuteStep(bool shouldExecute)
    {
        doNotExecuteStep = !shouldExecute;
    }

    #endregion
}
