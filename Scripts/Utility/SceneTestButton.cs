using UnityEngine;
using UnityEngine.Events;

public class SceneTestButton : MonoBehaviour
{
    [SerializeField] private RoomType _roomType;

    [SerializeField] private Material _teleportReadyMat;
    [SerializeField] private WallPusherManager _wallPusher;

    private Button3DInteractable _button;
    private Transform _teleportLocation;

    private bool _teleportReady;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponentInChildren<Button3DInteractable>();
        _button.OnButtonPressed.AddListener(OnPress);
    }

    private void OnPress()
    {
        if (_teleportReady)
        {
            if (_roomType == RoomType.EndCorridor)
            {
                _wallPusher.Push();
                return;
            }

            TeleportController.Instance.TeleportToPoint(_teleportLocation.position);
            return;
        }
        
        AsynSceneLoader.Instance.OnSceneLoaded += OnDoneLoadingScene;
        switch (_roomType)
        {
            case RoomType.Vacuum:
                _teleportLocation = World.Instance.VacuumVerseTeleportLocation;
                AsynSceneLoader.Instance.LoadScene(AsynSceneLoader.Scenes.VacuumVerse);
                break;
            case RoomType.Corpo:
                _teleportLocation = World.Instance.CorpoRoomTeleportLocation;
                AsynSceneLoader.Instance.LoadScene(AsynSceneLoader.Scenes.CorpoRoom);
                break;
            case RoomType.Path:
                _teleportLocation = World.Instance.RoomPathTeleportLocation;
                AsynSceneLoader.Instance.LoadScene(AsynSceneLoader.Scenes.RoomPath);
                break;
            case RoomType.Creation:
                _teleportLocation = World.Instance.CreationRoomTeleportLocation;
                AsynSceneLoader.Instance.LoadScene(AsynSceneLoader.Scenes.CreationRoom);
                break;
            case RoomType.EndCorridor:
                AsynSceneLoader.Instance.LoadScene(AsynSceneLoader.Scenes.EndCorridor);
                break;
            default:
                AsynSceneLoader.Instance.OnSceneLoaded -= OnDoneLoadingScene;
                break;
        }
    }

    private void OnDoneLoadingScene()
    {
        AsynSceneLoader.Instance.OnSceneLoaded -= OnDoneLoadingScene;
        _button.GetComponent<Renderer>().material = _teleportReadyMat;
        _teleportReady = true;
    }

    public enum RoomType
    {
        Vacuum,
        Corpo,
        Path,
        Creation,
        EndCorridor,
    }
}
