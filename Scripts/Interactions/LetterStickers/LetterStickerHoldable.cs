using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterStickerHoldable : StickerHoldable
{
    [SerializeField] private TextMeshProUGUI letterText;

    [SerializeField] private Letter letter;

    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

    public enum Letter
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
    }

    private Vector3 startingPos;

    public Vector3 StartingPos => startingPos;
    public Letter LetterType => letter;

    private bool _isValid;
    public bool IsValid => _isValid;

    private bool _enabledDropSfx;

    protected override void OnStart()
    {
        base.OnStart();
        
        letterText = GetComponentInChildren<TextMeshProUGUI>();

        letterText.text = letter.ToString();
        startingPos = transform.position;

        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        
        gameObject.tag = "LetterSticker";

        _isValid = false;
    }

    public void SetStickerValues(Letter letterType, Vector3 startingPos)
    {
        letter = letterType;
        this.startingPos = startingPos;

        this.transform.position = startingPos;
    }

    public void ToggleDropSFX(bool enabledSfx)
    {
        _enabledDropSfx = enabledSfx;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_enabledDropSfx) return;
        AudioManager.Instance.PlayOneShotBasicDrop(transform.position);
    }

    public void ToggleValid(bool valid)
    {
        _isValid = valid;
        letterText.color = valid ? validColor : invalidColor;
    }
}
