using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterStickerSystemManager : MonoBehaviour
{
    public static LetterStickerSystemManager Instance;

    [SerializeField] private LetterStickerIdentifierZone _idZone;

    public Action<string> OnSentenceChanged = (sentence) => { };

    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        else Instance = this;
    }

    private void OnEnable()
    {
        _idZone.OnSentenceChanged += SentenceChanged;
    }

    private void SentenceChanged(string newSentence)
    {
        OnSentenceChanged?.Invoke(newSentence);
    }

    private void OnDestroy()
    {
        _idZone.OnSentenceChanged -= SentenceChanged;
    }
}
