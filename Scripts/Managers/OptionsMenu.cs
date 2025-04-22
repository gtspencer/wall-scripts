using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _fpsLabel;
    [SerializeField] private TMP_Dropdown _fpsDropdown;
    [SerializeField] private Toggle _vsyncToggle;
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [SerializeField] private TextMeshProUGUI _wallText;
    
    // [SerializeField] private 

    private readonly Dictionary<int, int> _fpsValues = new Dictionary<int, int>
    {
        { 0, 30 },
        { 1, 60 },
        { 2, 120 },
        { 3, 240 },
        { 4, -1 },
    };

    private void OnEnable()
    {
        _fpsDropdown.value = _fpsValues.First(kv => kv.Value == SettingsManager.Instance.FPS).Key;
        _vsyncToggle.isOn = SettingsManager.Instance.VsyncEnabled == 1;
        if (_vsyncToggle.isOn)
        {
            _fpsDropdown.gameObject.SetActive(false);
            _fpsLabel.SetActive(false);
        }

        _qualityDropdown.value = SettingsManager.Instance.QualityIndex;
        
        _wallText.text = "SETTINGS";
        _fpsDropdown.onValueChanged.AddListener(OnFPSSet);
        _vsyncToggle.onValueChanged.AddListener(VsyncToggled);
        _qualityDropdown.onValueChanged.AddListener(OnQualitySet);
    }

    private void OnDisable()
    {
        _wallText.text = "WALL";
        
        _fpsDropdown.onValueChanged.RemoveListener(OnFPSSet);
        _vsyncToggle.onValueChanged.RemoveListener(VsyncToggled);
        _qualityDropdown.onValueChanged.RemoveListener(OnQualitySet);
    }

    private void VsyncToggled(bool syncEnabled)
    {
        SettingsManager.Instance.SetVsync(syncEnabled ? 1 : 0);

        _fpsDropdown.gameObject.SetActive(!syncEnabled);
        _fpsLabel.SetActive(!syncEnabled);

        if (!syncEnabled)
            SettingsManager.Instance.SetFPS(_fpsValues[_fpsDropdown.value]);
    }

    private void OnFPSSet(int arg)
    {
        var fps = _fpsValues[_fpsDropdown.value];

        SettingsManager.Instance.SetFPS(fps);
    }

    private void OnQualitySet(int arg)
    {
        SettingsManager.Instance.SetQuality(arg);
    }

    private void OnMasterVolumeSet(int arg)
    {
        SettingsManager.Instance.SetMasterVolume(arg);
    }
    
    private void OnSfxVolumeSet(int arg)
    {
        SettingsManager.Instance.SetSfxVolume(arg);
    }
    
    private void OnAmbientVolumeSet(int arg)
    {
        SettingsManager.Instance.SetAmbientVolume(arg);
    }
    
    private void OnMusicVolumeSet(int arg)
    {
        SettingsManager.Instance.SetMusicVolume(arg);
    }

    public void OnBackPressed()
    {
        Mainmenu.Instance.OnQuitOptions();
    }
}
