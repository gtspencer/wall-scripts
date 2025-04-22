using UnityEngine;

public class RoomPathEntryCoordinator : MonoBehaviour
{
    [SerializeField] private bool _shouldLoadScene;
    [SerializeField] private AsynSceneLoader.Scenes _sceneToLoad;
    
    [SerializeField] private bool _shouldUnloadScene;
    [SerializeField] private AsynSceneLoader.Scenes _sceneToUnload;
    
    private bool _playerEntered;

    public void OnPlayerEntered()
    {
        if (_playerEntered) return;

        _playerEntered = true;

        if (_shouldLoadScene)
            AsynSceneLoader.Instance.LoadScene(_sceneToLoad);
        
        if (_shouldUnloadScene)
            AsynSceneLoader.Instance.UnloadScene(_sceneToLoad);
    }
}
