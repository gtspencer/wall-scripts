using System.Collections;
using UnityEngine;

public class StageRoomController : MonoBehaviour
{
    [SerializeField] private Transform _leftSide;
    [SerializeField] private Transform _rightSide;
    
    private float _curtainDrawTime;
    [SerializeField] private float _curtainMoveDistance;

    [SerializeField] private AudioClip _curtainOpenClip;
    
    [SerializeField] private Transform _door;
    [SerializeField] private Vector3 _openDoorLocalPosition = new Vector3(-2.02999997f, 1.5f, 39.0953751f);

    [SerializeField] private AudioClip _doorOpenClip;
    
    public void DrawCurtains()
    {
        AudioManager.Instance.PlayOneShotSFXAudio(_curtainOpenClip, transform.position);
        _curtainDrawTime = _curtainOpenClip.length;
        StartCoroutine(DrawCurtains_Coroutine());
    }

    private IEnumerator DrawCurtains_Coroutine()
    {
        var elapsed = 0f;

        var leftStartPosition = _leftSide.localPosition;
        var rightStartPosition = _rightSide.localPosition;
        var rightEndPosition = rightStartPosition - new Vector3(0, 0, _curtainMoveDistance);
        var leftEndPosition = leftStartPosition + new Vector3(0, 0, _curtainMoveDistance);
        
        while (elapsed < _curtainDrawTime)
        {
            elapsed += Time.deltaTime;
            var t = elapsed / _curtainDrawTime;

            _leftSide.localPosition = Vector3.Lerp(leftStartPosition, leftEndPosition, t);
            _rightSide.localPosition = Vector3.Lerp(rightStartPosition, rightEndPosition, t);

            yield return null;
        }
    }

    public void OpenDoor()
    {
        AudioManager.Instance.PlayOneShotSFXAudio(_doorOpenClip, _door.position);
        StartCoroutine(ToggleDoor_Coroutine());
    }
    
    private IEnumerator ToggleDoor_Coroutine()
    {
        var elapsed = 0f;
        var duration = 1f;
        var startPosition = _door.localPosition;
        var endPosition = _openDoorLocalPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = elapsed / duration;

            _door.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            
            yield return null;
        }
    }
}
