using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LetterStickerIdentifierZone : MonoBehaviour
{
    private List<LetterStickerHoldable> validLetters = new List<LetterStickerHoldable>();
    private List<LetterStickerHoldable> _allLetters = new List<LetterStickerHoldable>();

    public Action<string> OnSentenceChanged = (sentence) => { };
    private string _currentSentence;

    public string CurrentSentence
    {
        get => _currentSentence;
        private set
        {
            var oldSentence = _currentSentence;

            _currentSentence = value;

            if (_currentSentence == oldSentence) return;
            
            OnSentenceChanged.Invoke(_currentSentence);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LetterSticker")) return;

        var letterSticker = other.GetComponent<LetterStickerHoldable>();

        letterSticker.ToggleValid(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("LetterSticker")) return;
        
        var letterSticker = other.GetComponent<LetterStickerHoldable>();
        
        letterSticker.ToggleValid(false);
    }

    private string ProcessLetters(List<LetterStickerHoldable> stickers)
    {
        var sorting = stickers.OrderBy(x => x.transform.position.x);

        string sentence = "";
        foreach (var letter in sorting)
        {
            sentence += letter.LetterType.ToString();
        }
        
        return sentence;
    }

    public void LogNewLetter(LetterStickerHoldable letter)
    {
        if (_allLetters.Contains(letter)) return;
        
        _allLetters.Add(letter);

        letter.OnHeldCallback += OnLetterHeld;
        letter.OnDroppedCallback += OnLetterDropped;
    }

    private void OnLetterHeld(Holdable holdable)
    {
        
    }

    private void OnLetterDropped(Holdable holdable)
    {
        var letter = holdable as LetterStickerHoldable;

        if (letter == null) return;

        if (letter.IsValid)
        {
            if (!validLetters.Contains(letter)) validLetters.Add(letter);

            GetSentence();
        }
        else
        {
            if (validLetters.Contains(letter))
            {
                validLetters.Remove(letter);
                GetSentence();
            }
        }
    }

    private string GetSentence()
    {
        CurrentSentence = ProcessLetters(validLetters);
        
        return _currentSentence;
    }

    public string GetInvalidLetters()
    {
        var invalidPlacedLetters = _allLetters.Where(l => !l.IsValid && l.StickerOnWall).ToList();
        
        return ProcessLetters(invalidPlacedLetters);
    }

    public void AllLettersFall()
    {
        foreach (var letter in _allLetters)
        {
            letter.ToggleDropSFX(true);
            letter.Rigidbody.useGravity = true;
            letter.Rigidbody.isKinematic = false;
            
            letter.Rigidbody.AddForce(new Vector3(0, 0, -1) * UnityEngine.Random.Range(50, 100));
        }
    }

    public void ClearAllLetters()
    {
        foreach (var letter in _allLetters)
        {
            letter.OnDroppedCallback -= OnLetterDropped;
            letter.OnHeldCallback -= OnLetterHeld;

            Destroy(letter.gameObject);
        }
        
        _allLetters.Clear();
    }

    private void OnDestroy()
    {
        foreach (var letter in _allLetters)
        {
            letter.OnDroppedCallback -= OnLetterDropped;
            letter.OnHeldCallback -= OnLetterHeld;
        }
    }
}
