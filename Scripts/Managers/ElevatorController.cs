using System;
using System.Collections;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private GameObject _player;
    private FirstPersonController _fpc;
    private CharacterController _cc;
    [SerializeField] private GameObject _backWall;
    [SerializeField] private Transform _teleportLocation;
    
    [Header("Screen Shake")]
    private CinemachineBasicMultiChannelPerlin _screenShakeComponent;

    [SerializeField] private float _shakeDuration = 5f;
    [SerializeField] private float _maxScreenShake = 1f;

    [Header("Doors")]
    [SerializeField] private float _doorMoveDistance = 6.99f;
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private float _doorOpenDuration = 1.5f;

    [Header("SFX")]
    [SerializeField] private AudioSource _ambient;
    [SerializeField] private AudioSource _doorOpen;
    [SerializeField] private AudioSource _ding;

    private void Start()
    {
        _fpc = FirstPersonController.Instance;
        _cc = _fpc.GetComponent<CharacterController>();

        _player = _fpc.gameObject;

        _screenShakeComponent = _fpc.CameraShake;
    }

    public void OnButtonPress()
    {
        StartCoroutine(ScreenShake());
        StartCoroutine(SFX());
        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds((_shakeDuration * 2) + 1);
        _doorOpen.Play();

        /*var elapsed = 0f;
        var leftStartPos = _leftDoor.localPosition;
        var leftTarget = leftStartPos - new Vector3(_doorMoveDistance, 0, 0);
        var rightStartPos = _rightDoor.localPosition;
        var rightTarget = rightStartPos + new Vector3(_doorMoveDistance, 0, 0);*/

        LeanTween.moveLocalX(_leftDoor.gameObject, _leftDoor.localPosition.x - _doorMoveDistance, _doorOpenDuration).setEaseOutBounce();
        LeanTween.moveLocalX(_rightDoor.gameObject, _rightDoor.localPosition.x + _doorMoveDistance, _doorOpenDuration).setEaseOutBounce();
        
        /*while (elapsed < _doorOpenDuration)
        {
            elapsed += Time.deltaTime;
            var t = elapsed / _doorOpenDuration;
            _leftDoor.localPosition = Vector3.Lerp(leftStartPos, leftTarget, t);
            _rightDoor.localPosition = Vector3.Lerp(rightStartPos, rightTarget, t);
            yield return null;
        }*/
    }

    private IEnumerator ScreenShake()
    {
        _backWall.SetActive(true);
        _screenShakeComponent.enabled = true;
        _screenShakeComponent.FrequencyGain = 0f;
        _screenShakeComponent.AmplitudeGain = 0.3f;

        var elapsed = 0f;
        while (elapsed < _shakeDuration)
        {
            elapsed += Time.deltaTime;
            var t = elapsed / _shakeDuration;

            _screenShakeComponent.FrequencyGain = t * _maxScreenShake;
            yield return null;
        }

        _fpc.enabled = false;
        _cc.enabled = false;
        _player.transform.SetParent(this.transform);
        transform.position = _teleportLocation.position;
        _player.transform.SetParent(null);
        _fpc.enabled = true;
        _cc.enabled = true;

        elapsed = 0f;
        while (elapsed < _shakeDuration)
        {
            elapsed += Time.deltaTime;
            var t = 1 - (elapsed / _shakeDuration);

            _screenShakeComponent.FrequencyGain = t * _maxScreenShake;
            yield return null;
        }

        _screenShakeComponent.FrequencyGain = 0f;
        _screenShakeComponent.enabled = false;
    }

    private IEnumerator SFX()
    {
        _ambient.Play();
        yield return new WaitForSeconds((_shakeDuration * 2) - 1);
        var elapsed = 0f;
        while (elapsed < 1)
        {
            elapsed += Time.deltaTime;
            var t = 1 - (elapsed / 1);

            _ambient.volume = t;
        }
        _ding.Play();
    }
}
