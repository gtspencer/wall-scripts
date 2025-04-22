using System;
using System.Collections;
using StarterAssets;
using UnityEngine;

public class CharacterSpeedController : MonoBehaviour
{
    public static CharacterSpeedController Instance;
    
    private FirstPersonController _fpsController;
    private float _defaultMoveSpeed;
    private float _defaultRotationSpeed;
    private float _defaultGravity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        _fpsController = GetComponent<FirstPersonController>();
        _defaultMoveSpeed = _fpsController.MoveSpeed;
        _defaultRotationSpeed = _fpsController.RotationSpeed;
        _defaultGravity = _fpsController.Gravity;
    }

    public void LerpSpeedToValue(float speedValue, float duration, float rotationValue = -1)
    {
        StartCoroutine(LerpSpeedToValue_Coroutine(speedValue, duration, rotationValue));
    }

    private IEnumerator LerpSpeedToValue_Coroutine(float speedValue, float duration, float rotationValue)
    {
        float startSpeed = _fpsController.MoveSpeed;
        float startRotationSpeed = _fpsController.RotationSpeed;
        float targetRotationSpeed = rotationValue < 0 ? startRotationSpeed : rotationValue;
        
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            _fpsController.MoveSpeed = Mathf.Lerp(startSpeed, speedValue, time / duration);
            _fpsController.RotationSpeed = Mathf.Lerp(startRotationSpeed, targetRotationSpeed, time / duration);
            
            yield return null;
        }
        
        _fpsController.MoveSpeed = speedValue;
        _fpsController.RotationSpeed = targetRotationSpeed;
    }
    
    public void LerpGravityToValue(float gravValue, float duration)
    {
        StartCoroutine(LerpGravityToValue_Coroutine(gravValue, duration));
    }

    private IEnumerator LerpGravityToValue_Coroutine(float gravValue, float duration)
    {
        float startGravity = _fpsController.Gravity;
        
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            _fpsController.Gravity = Mathf.Lerp(startGravity, gravValue, time / duration);
            
            yield return null;
        }
        
        _fpsController.Gravity = gravValue;
    }

    public void ResetSpeedVariables()
    {
        _fpsController.MoveSpeed = _defaultMoveSpeed;
        _fpsController.RotationSpeed = _defaultRotationSpeed;
        _fpsController.Gravity = _defaultGravity;
    }
}
