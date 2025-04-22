using System;
using System.Collections;
using UnityEngine;

public class LastRoomController : MonoBehaviour
{
    [SerializeField] private GameObject[] _lights;

    [SerializeField] private GameObject _tempWall;
    [SerializeField] private GameObject[] _buttonStuff;
    [SerializeField] private AudioClip _lightOnClip;

    [SerializeField] private GameObject[] _room;

    [SerializeField] private GameObject _wall;

    private TeleportPlayer _teleportPlayer;
    [SerializeField] private Event_WaitForEndButtonPress _waitEvent;

    [SerializeField] private Transform[] _wallCorners;
    [SerializeField] private GameObject _reverbZone;
    private Camera _camera;

    [SerializeField] private WallPusherManager _wallPusher;

    public void OnButtonPress()
    {
        _camera = Camera.main;
        StartCoroutine(OnButtonPress_Coroutine());
    }

    private IEnumerator OnButtonPress_Coroutine()
    {
        foreach (var room in _room)
        {
            room.SetActive(true);
        }
        
        yield return new WaitForSeconds(1.5f);

        while (AreTargetsOnScreen())
        {
            yield return null;
        }

        foreach (var buttonStuff in _buttonStuff)
            buttonStuff.SetActive(false);
        
        foreach (var light in _lights)
        {
            light.SetActive(false);
        }
        
        // TODO fade out ambient
        
        _wallPusher.Reset();

        yield return new WaitForSeconds(5f);
        
        TeleportController.Instance.TeleportToStart();
        _reverbZone.SetActive(true);
        
        AudioManager.Instance.PlayOneShotSFXAudio(_lightOnClip, new Vector3(0, 10, 0), volume: 1.2f, pitch: .6f);
        
        foreach (var light in _lights)
        {
            light.SetActive(true);
        }
        
        // TODO more text:
        // I missed my friend.
        MainWallCanvas.Instance.WriteImmediateSide1("You should not have come back");
        MainWallCanvas.Instance.WriteImmediateSide2("But I am glad you did");
        
        _waitEvent.OnButtonPressed();
    }
    
    private bool AreTargetsOnScreen()
    {
        // Find the min and max bounds of the AABB that will cover the four targets
        Vector3 min = _wallCorners[0].position;
        Vector3 max = _wallCorners[0].position;

        // Find the bounds of the AABB
        foreach (Transform target in _wallCorners)
        {
            min = Vector3.Min(min, target.position);
            max = Vector3.Max(max, target.position);
        }

        // Create an AABB using the min and max positions
        Bounds bounds = new Bounds((min + max) / 2, max - min);

        // Get the camera's planes
        Plane[] cameraPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);

        // Check if the AABB is within the camera's frustum
        return GeometryUtility.TestPlanesAABB(cameraPlanes, bounds);
    }
}
