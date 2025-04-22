using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _entryText;

    private int[] _validSequence = new int[4] { 1, 2, 3, 4 };

    private int _currentSequenceIndex = 0;
    private int[] _currentSequence = new int[4] {-1, -1, -1, -1};

    private Button3DInteractable[] _buttons;

    [SerializeField] private GameObject _staticDoor;
    [SerializeField] private Door _door;
    [SerializeField] private FlickeringLight _flickeringLight;

    private Dictionary<string, string> _inputsToOutputs = new Dictionary<string, string>
    {
        { "6969", "perv" },
        { "8008", "perv" }
    };

    private void OnEnable()
    {
        _buttons = GetComponentsInChildren<Button3DInteractable>();
    }

    public void OnNumberPressed(int number)
    {
        _currentSequence[_currentSequenceIndex] = number;

        _currentSequenceIndex++;
        UpdateText();
        
        // last index
        if (_currentSequenceIndex == 4)
            CheckDoorCode();
    }

    private void CheckDoorCode()
    {
        if (_validSequence.SequenceEqual(_currentSequence))
            ConfirmDoorOpen();
        else
            ResetDoorCode();
    }

    private void ConfirmDoorOpen()
    {
        _staticDoor.SetActive(false);
        _door.gameObject.SetActive(true);
        ToggleButtonInteractable(false);
        _entryText.color = Color.green;
        _door.Open();
        _flickeringLight.FadeInSFX();
    }

    private void ResetDoorCode()
    {
        StartCoroutine(ResetDoorCode_Coroutine());
    }

    private IEnumerator ResetDoorCode_Coroutine()
    {
        ToggleButtonInteractable(false);
        
        yield return new WaitForSeconds(0.3f);

        _entryText.color = Color.red;
        if (_inputsToOutputs.TryGetValue(_entryText.text, out var val))
        {
            yield return new WaitForSeconds(1f);
            _entryText.text = val;
        }

        yield return new WaitForSeconds(2f);
        _entryText.text = "";
        _currentSequenceIndex = 0;
        _currentSequence = new int[4] {-1, -1, -1, -1};

        _entryText.color = Color.white;
        ToggleButtonInteractable(true);
    }

    private void ToggleButtonInteractable(bool interactable)
    {
        foreach (var button in _buttons)
        {
            button.CanInteract = interactable;
        }
    }

    private void UpdateText()
    {
        _entryText.text = ConvertArrayToString(_currentSequence);
    }
    
    private string ConvertArrayToString(int[] numbers)
    {
        return string.Concat(numbers.Where(n => n != -1));
    }

    [Header("On entry/exit vars")]
    [SerializeField] private AudioReverbZone _reverbZone;
    [SerializeField] private TextMeshProUGUI _wallText;
    public void OnEnterRoom()
    {
        _reverbZone.enabled = true;
        
        _wallText.text = "Through the greatest escape.";
    }

    public void OnExitRoom()
    {
        _reverbZone.enabled = false;
        
        _wallText.text = "Through sheer will.";
    }
}
