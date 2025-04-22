using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class ScriptedEventController : MonoBehaviour
{
    // I put all the events in individual gameobjects underneath a parent so i can just grab by type
    // i do this to make it easy for events to use serialized references in scene instead of having to pass a bunch of relevant references every time i need a new step
    // why don't i use dependency injection for this?
    // fuck you thats why
    [SerializeField] private Transform scriptedEventsRoot;
    private List<ScriptedEvent> scriptedEvents = new List<ScriptedEvent>();

    private int _currentEventIndex = 0;
    
    [FormerlySerializedAs("side1Trigger")] [SerializeField] private SideTriggerCollisionReporter side1SideTrigger;
    [FormerlySerializedAs("side2Trigger")] [SerializeField] private SideTriggerCollisionReporter side2SideTrigger;
    
    // TODO remove from build
    [Header("Test")] [SerializeField]
    private bool useTestEvents;
    [ShowIf("useTestEvents")] [SerializeField]
    private Transform testEventsRoot;
    [Header("Jump Steps")]
    [SerializeField]
    private int stepsToJump = 5;
    
    public static ScriptedEventController Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    
    private ScriptedEvent CurrentEvent => scriptedEvents[_currentEventIndex];
    
    private bool _completed;

    private void Start()
    {
        if (useTestEvents)
            scriptedEventsRoot = testEventsRoot;
        
        PopulateEvents();
        
        CurrentEvent.OnEventComplete += OnEventCompleted;
        CurrentEvent.StartEvent();
    }

    public void TogglePaused(bool pause)
    {
        MainWallCanvas.Instance.ToggleTextHidden(pause);
        
        side1SideTrigger.TogglePaused(pause);
        side2SideTrigger.TogglePaused(pause);
    }

    private void PopulateEvents()
    {
        scriptedEvents = new List<ScriptedEvent>();

        // this ensures proper ordering
        foreach (Transform t in scriptedEventsRoot)
        {
            var child = t.GetComponents<ScriptedEvent>();
            foreach (var e in child)
                if (e != null && e.isActiveAndEnabled) scriptedEvents.Add(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_completed) return;
        
        CurrentEvent.UpdateEvent();
    }
    
    [Button("Jump Steps", EButtonEnableMode.Playmode)]
    private void JumpStep(int stepsToJump = -1)
    {
        if (stepsToJump <= -1)
            stepsToJump = this.stepsToJump;
        
        for (int i = 0; i < stepsToJump; i++)
        {
            CurrentEvent.OnEventComplete.Invoke(CurrentEvent);
        }
    }

    public void InsertEventsAfterCurrent(List<ScriptedEvent> events)
    {
        scriptedEvents.InsertRange(_currentEventIndex + 1, events);
    }

    private void NextEvent()
    {
        CurrentEvent.EndEvent();

        var completeMessage = "Completed event " + CurrentEvent + ".  Total time: " + CurrentEvent.GetTimeInEvent();
        Debug.Log(completeMessage);
        PlayerInfo.Instance.AddEventMessage(completeMessage);
        
        _currentEventIndex++;

        if (_currentEventIndex >= scriptedEvents.Count)
        {
            SimulationComplete();
            return;
        }

        CurrentEvent.OnEventComplete += OnEventCompleted;

        var startMessage = "Starting event " + CurrentEvent;
        Debug.Log(startMessage);
        PlayerInfo.Instance.AddEventMessage(startMessage);
        
        CurrentEvent.StartEvent();
    }

    private void OnEventCompleted(ScriptedEvent completedEvent)
    {
        completedEvent.OnEventComplete -= OnEventCompleted;
        
        NextEvent();
    }
    
    private void SimulationComplete()
    {
        Debug.Log("Simulation complete");
        _completed = true;
    }
}
