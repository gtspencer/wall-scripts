using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WallPusherManager : MonoBehaviour
{
    [SerializeField] private WallPusher[] _pushers;
    [SerializeField] private GameObject[] _staticWalls;

    [SerializeField] private GameObject[] _externalRooms;

    [Button]
    public void Push()
    {
        AmbientAudioManager.Instance.FadeInAmbient(AmbientAudioTypes.Ending);
        foreach (var wall in _staticWalls)
            wall.SetActive(false);
        
        foreach (var pusher in _pushers)
        {
            pusher.gameObject.SetActive(true);
            pusher.Push();
        }
        
        StartCoroutine(EnableRoomsAsync());
    }

    private IEnumerator EnableRoomsAsync()
    {
        foreach (var room in _externalRooms)
        {
            yield return null;
            yield return null;
            room.SetActive(true);
        }
    }

    public void Reset()
    {
        foreach (var pusher in _pushers)
        {
            pusher.gameObject.SetActive(false);
        }
        
        foreach (var wall in _staticWalls)
            wall.SetActive(true);
    }
}
