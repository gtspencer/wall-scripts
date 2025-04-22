using UnityEngine;

public class SciptedEvent_LoadScene : ScriptedEvent
{
    [SerializeField] private AsynSceneLoader.Scenes _sceneToLoad;
    protected override void OnStart()
    {
        AsynSceneLoader.Instance.LoadScene(_sceneToLoad);
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
