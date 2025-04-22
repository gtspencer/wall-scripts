using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    /*public enum KeyType
    {
        C,
        CSharp,
        D,
        DSharp,
        E,
        F,
        FSharp,
        G,
        GSharp,
        A,
        ASharp,
        B
    }*/

    // [SerializeField] private KeyType keyType;
    [SerializeField] private AudioClip keyClip;

    private Button3DInteractable _button;
    private int _keyIndex;

    public Action<int> OnKeyPressed = (index) => { };
    
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponentInChildren<Button3DInteractable>();

        _button.OnButtonPressed.AddListener(KeyPressed);
    }

    private void KeyPressed()
    {
        AudioManager.Instance.PlayOneShotSFXAudio(keyClip, transform.position);

        OnKeyPressed?.Invoke(_keyIndex);
    }

    public void ToggleInteractable(bool canInteract)
    {
        _button.CanInteract = canInteract;
    }

    public void PressKey()
    {
        _button.Interact();
    }

    public void SetKeyIndex(int index)
    {
        _keyIndex = index;
    }
}
