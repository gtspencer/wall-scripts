using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup sfxGroup;
    
    #region Music
    [Header("Music")]
    public AudioSource ambientAudioSource;
    public AudioSource musicAudioSource;
    #endregion

    #region SFX
    [Header("SFX")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource wallAudioSource;
    #endregion

    private AudioClip[] _basicDropSounds = new AudioClip[]{};
    private AudioClip[] _splatSounds = new AudioClip[]{};

    // 0 - 100
    private int _currentMasterVolume;
    public int CurrentMasterVolume
    {
        get => _currentMasterVolume;
        set
        {
            _currentMasterVolume = value;
            
            mainMixer.SetFloat("masterVol", NormalizedValueToVolume(value));
        }
    }

    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);

        Instance = this;
    }

    private void Start()
    {
        if (mainMixer.GetFloat("masterVol", out var volume))
        {
            CurrentMasterVolume = VolumeToNormalizedValue(volume);
        }

        _basicDropSounds = Resources.LoadAll<AudioClip>("Audio/SFX/ItemDrops");
        _splatSounds = Resources.LoadAll<AudioClip>("Audio/SFX/Splats");
    }

    public void PlaySFXOnPlayer(AudioClip clip)
    {
        playerAudioSource.clip = clip;
        playerAudioSource.Play();
    }

    public void PlaySFXOnWall(AudioClip clip)
    {
        wallAudioSource.clip = clip;
        wallAudioSource.Play();
    }

    public GameObject PlayOneShotSFXAudio(AudioClip clip, Vector3 location, float volume = 1, float pitch = 1, bool returnWhenDone = true)
    {
        var audioSourceGameObject = SFXAudioSourcePool_SFX.Instance.GetAudioSource();
        var audioSource = audioSourceGameObject.GetComponent<AudioSource>();

        audioSourceGameObject.transform.position = location;

        audioSource.loop = false;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        if (returnWhenDone)
            StartCoroutine(ReturnAudioSourceWhenDonePlaying(audioSourceGameObject, clip.length));

        return audioSourceGameObject;
    }

    public GameObject PlayOneShotBasicDrop(Vector3 location, float volume = 1, float pitch = 1,
        bool returnWhenDone = true)
    {
        var clip = _basicDropSounds[Random.Range(0, _basicDropSounds.Length - 1)];

        return PlayOneShotSFXAudio(clip, location, volume, pitch, returnWhenDone);
    }
    
    public GameObject PlayOneShotSplat(Vector3 location, float volume = 1, float pitch = 1,
        bool returnWhenDone = true)
    {
        var clip = _splatSounds[Random.Range(0, _splatSounds.Length - 1)];

        return PlayOneShotSFXAudio(clip, location, volume, pitch, returnWhenDone);
    }

    public void ReturnOneShotSFXAudio(GameObject obj)
    {
        SFXAudioSourcePool_SFX.Instance.ReturnAudioSource(obj);
    }

    private IEnumerator ReturnAudioSourceWhenDonePlaying(GameObject audioSource, float duration)
    {
        yield return new WaitForSeconds(duration * 1.5f);

        SFXAudioSourcePool_SFX.Instance.ReturnAudioSource(audioSource);
    }

    private const float minVolume = -80f;
    private const float maxVolume = 0f;

    // normalized values are between 0 and 100
    private float NormalizedValueToVolume(int value)
    {
        var range = maxVolume - minVolume;

        var progress = value * range / 100;

        var volumeValue = progress + minVolume;

        return volumeValue;
    }
    
    private int VolumeToNormalizedValue(float volume)
    {
        var totalRange = Mathf.Abs(maxVolume - minVolume);
        var normalized = Mathf.Abs((minVolume - volume) / totalRange);
        
        var value = (int)(normalized * 100);
        
        return value;
    }

    // 0 - 100
    public void SetMasterVolume(int value)
    {
        mainMixer.SetFloat("masterVol", NormalizedValueToVolume(value));
    }
    
    public void SetSfxVolume(int value)
    {
        sfxGroup.audioMixer.SetFloat("sfxVol", NormalizedValueToVolume(value));
    }
    
    public void SetMusicVolume(int value)
    {
        sfxGroup.audioMixer.SetFloat("musicVol", NormalizedValueToVolume(value));
    }
    
    public void SetAmbientVolume(int value)
    {
        sfxGroup.audioMixer.SetFloat("ambientVol", NormalizedValueToVolume(value));
    }
}
