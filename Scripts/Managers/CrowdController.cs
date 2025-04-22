using System.Collections;
using NaughtyAttributes;
using StarterAssets;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
    private AudioSource _crowdSource;

    [SerializeField] private Vector3 _leftArmBlockRotation;
    [SerializeField] private Transform _leftArm;
    private Quaternion _leftArmStartPosition;
    [SerializeField] private Vector3 _rightArmBlockRotation;
    [SerializeField] private Transform _rightArm;
    private Quaternion _rightArmStartPosition;

    [SerializeField]
    private AudioProximityVolume _audioProximityVolume;

    [SerializeField] private GameObject _blockingCollider;

    private Coroutine _audioFadeCoroutine;
    private Coroutine _armRaiseCoroutine;
    private FirstPersonController fpc;

    private bool _playerStopped;

    [SerializeField]
    private LookAtPlayer[] _lookAtPlayer;
    private Animator[] _animators;
    private CrowdedPerson[] _crowdedPersons;

    [Header("Player Move Variables")]
    [SerializeField] private float _playerMoveSpeed = 2f;
    [SerializeField] private float _movePlayerAfterTime = 5f;
    [SerializeField] private Transform _playerExitPoint;
    private Vector3 _playerExitPosition;

    private bool _started;

    private bool PlayerStopped
    {
        get => _playerStopped;
        set
        {
            if (_playerStopped == value) return;
            
            _playerStopped = value;

            if (_started && !_ended)
            {
                FadeAudioAndAnim(fadeOut: !_playerStopped);
                HandlePlayerFloat(_playerStopped);

                foreach (var person in _crowdedPersons)
                {
                    if (_playerStopped)
                        person.FadeToFace();
                    else
                        person.ISeeYou();
                }
                    
            }
        }
    }
    
    // private bool PlayerStopped => fpc.CurrentSpeed <= 0.01f;
    // private bool _
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fpc = FirstPersonController.Instance;
        _crowdSource = _audioProximityVolume.GetComponent<AudioSource>();

        // _lookAtPlayer = GetComponentsInChildren<LookAtPlayer>();
        
        _animators = new Animator[_lookAtPlayer.Length];
        _crowdedPersons = new CrowdedPerson[_lookAtPlayer.Length];
        
        for (int i = 0; i < _lookAtPlayer.Length; i++)
        {
            _lookAtPlayer[i].enabled = false;
            _animators[i] = _lookAtPlayer[i].GetComponent<Animator>();
            _animators[i].enabled = false;
            _crowdedPersons[i] = _lookAtPlayer[i].GetComponent<CrowdedPerson>();

            StartCoroutine(PlayRandomly_Coroutine(_animators[i]));
        }

        _leftArmStartPosition = _leftArm.transform.localRotation;
        _rightArmStartPosition = _rightArm.transform.localRotation;
        
        _playerExitPosition = new Vector3(_playerExitPoint.position.x, fpc.transform.position.y, _playerExitPoint.position.z);
    }

    private IEnumerator PlayRandomly_Coroutine(Animator anim)
    {
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        anim.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerStopped = fpc.CurrentInput <= 0.01f;
    }

    private Coroutine _playerFloatCoroutine;
    private void HandlePlayerFloat(bool playerStopped)
    {
        if (_playerFloatCoroutine != null)
            StopCoroutine(_playerFloatCoroutine);
        
        if (playerStopped)
        {
            _playerFloatCoroutine = StartCoroutine(PlayerFloat_Coroutine());
        }
    }

    private IEnumerator PlayerFloat_Coroutine()
    {
        yield return new WaitForSeconds(_movePlayerAfterTime);

        while (Vector3.Distance(_playerExitPosition, fpc.transform.position) > 0.1f)
        {
            fpc.transform.position = Vector3.MoveTowards(
                fpc.transform.position,
                _playerExitPosition,
                _playerMoveSpeed * Time.deltaTime
            );
            
            yield return null;
        }
    }
    
    public void StartSequence()
    {
        if (_started) return;
        
        _audioProximityVolume.DontSetVolumeHere = true;
        _started = true;

        if (!PlayerStopped)
            FadeAudioAndAnim(true);
    }

    private bool _ended;
    public void EndSequence()
    {
        if (_ended) return;
        
        _ended = true;

        if (_audioFadeCoroutine != null)
        {
            StopCoroutine(_audioFadeCoroutine);
            _audioFadeCoroutine = null;
        }

        if (_armRaiseCoroutine != null)
        {
            StopCoroutine(_armRaiseCoroutine);
            _armRaiseCoroutine = null;
        }

        _blockingCollider.SetActive(true);
        _audioProximityVolume.DontSetVolumeHere = false;
        StartCoroutine(RaiseArms_Coroutine(true));

        foreach (var lap in _lookAtPlayer)
            lap.enabled = false;
    }

    private void FadeAudioAndAnim(bool fadeOut)
    {
        _blockingCollider.SetActive(fadeOut);
        
        if (_audioFadeCoroutine != null)
        {
            StopCoroutine(_audioFadeCoroutine);
            _audioFadeCoroutine = null;
        }
        
        if (_armRaiseCoroutine != null)
        {
            StopCoroutine(_armRaiseCoroutine);
            _armRaiseCoroutine = null;
        }
        
        foreach (var lap in _lookAtPlayer)
        {
            lap.enabled = fadeOut;
        }
        
        _audioFadeCoroutine = StartCoroutine(FadeAudioAndAnim_Coroutine(fadeOut));
        _armRaiseCoroutine = StartCoroutine(RaiseArms_Coroutine(fadeOut));
    }

    [SerializeField] private float _audioFadeTime = 0.5f;
    [SerializeField] private float _armRaiseTime = 0.1f;
    private IEnumerator FadeAudioAndAnim_Coroutine(bool fadeOut)
    {
        var elapsed = 0f;
        while (elapsed < _audioFadeTime)
        {
            elapsed += Time.deltaTime;

            var t = elapsed / _audioFadeTime;
            if (fadeOut)
                t = 1 - t;

            _crowdSource.volume = _audioProximityVolume.CurrentVolumeValue * t;

            for (int i = 0; i < _lookAtPlayer.Length; i++)
            {
                _animators[i].speed = t;
            }
            
            yield return null;
        }
    }
    
    private IEnumerator RaiseArms_Coroutine(bool fadeOut)
    {
        var leftStart = _leftArm.transform.localRotation;
        var rightStart = _rightArm.transform.localRotation;

        var leftEnd = fadeOut ? Quaternion.Euler(_leftArmBlockRotation) : _leftArmStartPosition;
        var rightEnd = fadeOut ? Quaternion.Euler(_rightArmBlockRotation) : _rightArmStartPosition;
        
        var elapsed = 0f;
        while (elapsed < _armRaiseTime)
        {
            elapsed += Time.deltaTime;

            var rawT = elapsed / _armRaiseTime;

            _leftArm.transform.localRotation = Quaternion.Lerp(leftStart, leftEnd, rawT);
            _rightArm.transform.localRotation = Quaternion.Lerp(rightStart, rightEnd, rawT);
            
            yield return null;
        }
    }

    [Button]
    private void ArmsBlock()
    {
        Debug.LogError("Left: " + _leftArm.transform.localRotation);
        Debug.LogError("right: " + _rightArm.transform.localRotation);
    }
}
