using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StarterAssets;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    // [SerializeField]
    // private StarterAssetsInputs _input;

    [SerializeField] private GameObject _reticle;
    private bool previousReticleState;

    private bool _menuDisplayed;

    [Header("Roots")]
    [SerializeField] private GameObject side1Root;
    [SerializeField] private GameObject side2Root;
    
    [Header("Volume")]
    [SerializeField] private SliderHoldable mainVolumeSliderSide1;
    [SerializeField] private SliderHoldable mainVolumeSliderSide2;

    private void Start()
    {
        mainVolumeSliderSide1.OnValueChanged += VolumeUpdated;
        mainVolumeSliderSide2.OnValueChanged += VolumeUpdated;

        InputRepository.Instance.OnEscape += DetermineMenuState;
    }

    private void VolumeUpdated(int value)
    {
        AudioManager.Instance.CurrentMasterVolume = value;
    }

    [Button]
    private void DetermineMenuState()
    {
        _menuDisplayed = !_menuDisplayed;
        ScriptedEventController.Instance.TogglePaused(_menuDisplayed);

        if (!_menuDisplayed)
        {
            HideMenus();
        }
        else
        {
            ShowMenus();
        }
    }

    private void HideMenus()
    {
        _reticle.SetActive(previousReticleState);
        
        side1Root.SetActive(false);
        side2Root.SetActive(false);
    }

    private void ShowMenus()
    {
        previousReticleState = _reticle.activeSelf;
        _reticle.SetActive(true);
        
        side1Root.SetActive(true);
        side2Root.SetActive(true);
        
        mainVolumeSliderSide1.SetupInitialValue(AudioManager.Instance.CurrentMasterVolume);
        mainVolumeSliderSide2.SetupInitialValue(AudioManager.Instance.CurrentMasterVolume);
    }
}
