using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
        
        GetPlayerPrefs();
    }

    public int FPS => _fps;
    private int _fps;

    public int QualityIndex => _qualityIndex;
    private int _qualityIndex;
    
    public int VsyncEnabled => _vsyncEnabled;
    private int _vsyncEnabled;

    public int MasterVolume => _masterVolume;
    private int _masterVolume;
    public int SfxVolume => _sfxVolume;
    private int _sfxVolume;
    public int AmbientVolume => _ambientVolume;
    private int _ambientVolume;
    public int MusicVolume => _musicVolume;
    private int _musicVolume;

    
    private void GetPlayerPrefs()
    {
        var fps = PlayerPrefs.GetInt("fps", 120);
        SetFPS(fps);
        
        var vsync = PlayerPrefs.GetInt("vsync", 0);
        SetVsync(vsync);

        var quality = PlayerPrefs.GetInt("quality", 0);
        SetQuality(quality);

        var masterVolume = PlayerPrefs.GetInt("masterVol", 100);
        SetMasterVolume(masterVolume);
        
        var sfxVolume = PlayerPrefs.GetInt("sfxVol", 100);
        SetSfxVolume(sfxVolume);
        
        var ambientVolume = PlayerPrefs.GetInt("ambientVol", 100);
        SetAmbientVolume(ambientVolume);
        
        var musicVolume = PlayerPrefs.GetInt("musicVol", 100);
        SetMusicVolume(musicVolume);
    }

    public void SetFPS(int fps)
    {
        _fps = fps;

        Application.targetFrameRate = fps;
        
        PlayerPrefs.SetInt("fps", fps);
    }

    public void SetQuality(int index)
    {
        _qualityIndex = index;

        QualitySettings.SetQualityLevel(index, true);
        
        PlayerPrefs.SetInt("quality", index);
    }

    public void SetVsync(int syncEnabled)
    {
        _vsyncEnabled = syncEnabled;

        QualitySettings.vSyncCount = syncEnabled;
        
        PlayerPrefs.SetInt("vsync", syncEnabled);
    }
    
    public void SetMasterVolume(int value)
    {
        _masterVolume = value;
        
        PlayerPrefs.SetInt("masterVol", value);

        AudioManager.Instance.SetMasterVolume(value);
    }
    
    public void SetSfxVolume(int value)
    {
        _sfxVolume = value;
        
        PlayerPrefs.SetInt("sfxVol", value);
        
        AudioManager.Instance.SetSfxVolume(value);
    }
    
    public void SetAmbientVolume(int value)
    {
        _ambientVolume = value;
        
        PlayerPrefs.SetInt("ambientVol", value);
        
        AudioManager.Instance.SetAmbientVolume(value);
    }
    
    public void SetMusicVolume(int value)
    {
        _musicVolume = value;
        
        PlayerPrefs.SetInt("musicVol", value);
        
        AudioManager.Instance.SetMusicVolume(value);
    }
}
