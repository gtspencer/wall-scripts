using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class GiantPiano : MonoBehaviour
{
    private List<PianoKey> keys = new List<PianoKey>();
    [SerializeField] private float timeBetweenAutoKeyPresses_sec = 1f;

    [SerializeField] [Tooltip("Starting number of sequence")]
    private int _startingCount = 3;
    private int _currentCorrectCount = 0;

    private int[] _keysToWaitFor;

    private int _keysPressedCounter;

    [SerializeField] private AudioClip _succeedClip;
    [SerializeField] private AudioClip _succeedClipShort;
    [SerializeField] private AudioClip[] _failClips;

    private int _failClipIndex = 0;

    [SerializeField] private AudienceMannequin[] _audience;

    [SerializeField] private StageRoomController _stageRoomController;
    // Start is called before the first frame update
    void Start()
    {
        keys = GetComponentsInChildren<PianoKey>().ToList();

        for (int i = 0; i < keys.Count; i++)
            keys[i].SetKeyIndex(i);
        
        foreach (var key in keys)
            key.OnKeyPressed += AnyKeyPressed;

        _currentCorrectCount = _startingCount;
    }

    [SerializeField] private GameObject _theMusicalText;
    private bool _anyKeyWasPressed;
    private float _startSequenceWaitTime = 5f;
    private Coroutine _waitToStartSequenceCoroutine;
    private void AnyKeyPressed(int index)
    {
        if (_sequenceStarted) return;

        if (_waitToStartSequenceCoroutine != null)
            StopCoroutine(_waitToStartSequenceCoroutine);
        
        _waitToStartSequenceCoroutine = StartCoroutine(WaitToStartSequence_Coroutine());
    }

    private bool _sequenceStarted;

    private IEnumerator WaitToStartSequence_Coroutine()
    {
        yield return new WaitForSeconds(_startSequenceWaitTime);

        _sequenceStarted = true;
        
        foreach (var key in keys)
            key.OnKeyPressed -= AnyKeyPressed;

        PlaySequence();
    }

    [Button]
    public void PlaySequence()
    {
        _keysPressedCounter = 0;
        ToggleKeysInteractable(false);

        var keysToPress = new int[_currentCorrectCount];

        for (int i = 0; i < _currentCorrectCount; i++)
        {
            keysToPress[i] = UnityEngine.Random.Range(0, keys.Count);
        }

        StartCoroutine(PlaySequence_Coroutine(keysToPress));
    }

    private IEnumerator PlaySequence_Coroutine(int[] keyIndexes)
    {
        foreach (var i in keyIndexes)
        {
            keys[i].PressKey();
            yield return new WaitForSeconds(timeBetweenAutoKeyPresses_sec);
        }

        _keysToWaitFor = keyIndexes;
        SetWaitForSequence();
    }

    private void SetWaitForSequence()
    {
        foreach (var key in keys)
            key.OnKeyPressed += KeyPressed;

        ToggleKeysInteractable(true);
    }

    private void CleanupKeyWaiting()
    {
        foreach (var key in keys)
            key.OnKeyPressed -= KeyPressed;
    }

    private void KeyPressed(int keyIndex)
    {
        // incorrect key
        if (_keysToWaitFor[_keysPressedCounter] != keyIndex)
        {
            FailedSequence();
            return;
        }
        
        // correct key, so increment
        _keysPressedCounter++;
        
        // did entire length, set succeeded
        if (_keysPressedCounter == _keysToWaitFor.Length)
            SucceedSequence();
    }

    private void SucceedSequence()
    {
        CleanupKeyWaiting();
        
        _currentCorrectCount++;
        
        

        // they've completed 5 keys
        if (_currentCorrectCount >= 6)
        {
            AudioManager.Instance.PlayOneShotSFXAudio(_succeedClip, transform.position);
            _stageRoomController.OpenDoor();
            ThrowRoses();
            _theMusicalText.SetActive(true);
            return;
        }
        
        AudioManager.Instance.PlayOneShotSFXAudio(_succeedClipShort, transform.position);
        
        StartCoroutine(PlaySequenceAfterSeconds(_succeedClipShort.length));
    }

    private IEnumerator PlaySequenceAfterSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime + 1f);
        PlaySequence();
    }

    private void FailedSequence()
    {
        CleanupKeyWaiting();

        if (_failClipIndex >= _failClips.Length)
            _failClipIndex = _failClips.Length - 1;
        
        var clip = _failClips[_failClipIndex];
        AudioManager.Instance.PlayOneShotSFXAudio(clip, transform.position);
        _failClipIndex++;

        StartCoroutine(WaitForBooingToStop(clip.length));
        ThrowTomatoes();
    }

    [Button]
    private void ThrowTomatoes()
    {
        foreach (var audience in _audience)
            audience.ThrowTomato(OnDoneThrowing);
    }

    [Button]
    private void ThrowRoses()
    {
        foreach (var audience in _audience)
            audience.ThrowRose();
    }

    private bool _donePlayingBoos;
    private IEnumerator WaitForBooingToStop(float duration)
    {
        yield return new WaitForSeconds(duration);
        _donePlayingBoos = true;
        
        CheckDoneFailSequence();
    }

    private int _doneThrowingCount;
    private void OnDoneThrowing()
    {
        _doneThrowingCount++;

        CheckDoneFailSequence();
    }

    private void CheckDoneFailSequence()
    {
        if (_doneThrowingCount >= _audience.Length && _donePlayingBoos)
        {
            _donePlayingBoos = false;
            _doneThrowingCount = 0;
            
            StartCoroutine(PlaySequenceAfterSeconds(2f));
        }
    }

    private void ToggleKeysInteractable(bool canInteract)
    {
        foreach (var key in keys)
        {
            key.ToggleInteractable(canInteract);
        }
    }
}
