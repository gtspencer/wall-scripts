using UnityEngine;

public class PlayerSpeedModifyEvent : ScriptedEvent
{
    [Header("Speed")]
    [SerializeField] private bool _resetSpeed;
    [SerializeField] private float _duration;
    [SerializeField] private float _targetMovementSpeed;
    [SerializeField] private float _targetRotationSpeed;

    [Header("Gravity")]
    [SerializeField] private float _targetGravity;
    
    private CharacterSpeedController _characterSpeedController;
    protected override void OnStart()
    {
        _characterSpeedController = CharacterSpeedController.Instance;
        
        if (_resetSpeed)
            _characterSpeedController.ResetSpeedVariables();
        else
        {
            _characterSpeedController.LerpSpeedToValue(_targetMovementSpeed, _duration, _targetRotationSpeed);
            _characterSpeedController.LerpGravityToValue(_targetGravity, _duration);
        }
            

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
