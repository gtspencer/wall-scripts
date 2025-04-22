using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterStickerSpawner : MonoBehaviour
{
    [SerializeField] private List<LetterStickerHoldable> letterStickers = new List<LetterStickerHoldable>();

    [SerializeField] private GameObject LetterStickerPrefab;

    [SerializeField] private LetterStickerIdentifierZone letterZone;

    private void Start()
    {
        foreach (var letterSticker in letterStickers)
        {
            letterSticker.OnHeldCallback += LetterStickerHeld;
        }
    }

    private void LetterStickerHeld(Holdable holdable)
    {
        holdable.OnHeldCallback -= LetterStickerHeld;
        
        var oldLetterSticker = (LetterStickerHoldable)holdable;
        
        if (oldLetterSticker == null) return;

        // set sticker color red
        oldLetterSticker.ToggleValid(false);
        // log reference to all placed stickers
        letterZone.LogNewLetter(oldLetterSticker);

        // replace sticker on wall so player can pick up new identical letters
        var newLetterSticker = Instantiate(LetterStickerPrefab, this.transform);

        var letterStickerInteractable = newLetterSticker.GetComponent<LetterStickerHoldable>();
        
        letterStickerInteractable.SetStickerValues(oldLetterSticker.LetterType, oldLetterSticker.StartingPos);
        
        letterStickerInteractable.OnHeldCallback += LetterStickerHeld;
    }
}
