using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MusicAudio
{
    public MusicAudioTypes AmbientType;
    public AudioClip Clip;
    public float StartVolume;
    public float EndVolume;
    public float Duration;
}

public enum MusicAudioTypes
{
    Piano,
}

public class MusicAudioManager : MonoBehaviour
{
    public static MusicAudioManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        foreach (var musicAudio in _musicAudiosRaw)
            _musicAudios.Add(musicAudio.AmbientType, musicAudio);
    }

    [SerializeField]
    private MusicAudio[] _musicAudiosRaw;

    private Dictionary<MusicAudioTypes, MusicAudio>
        _musicAudios = new Dictionary<MusicAudioTypes, MusicAudio>();

    public void PlayEndingMusicAudio()
    {
        FadeInAmbient(MusicAudioTypes.Piano);
    }

    public void FadeInAmbient(MusicAudioTypes audioType)
    {
        StartCoroutine(FadeInAmbient_Coroutine(audioType));
    }

    private IEnumerator FadeInAmbient_Coroutine(MusicAudioTypes audioType)
    {
        if (!_musicAudios.ContainsKey(audioType)) yield return null;

        var musicAudio = _musicAudios[audioType];

        AudioManager.Instance.musicAudioSource.clip = musicAudio.Clip;
        AudioManager.Instance.musicAudioSource.volume = musicAudio.StartVolume;
        AudioManager.Instance.musicAudioSource.Play();
        
        var elapsedTime = 0f;
        
        while (elapsedTime < musicAudio.Duration)
        {
            AudioManager.Instance.musicAudioSource.volume = Mathf.Lerp(musicAudio.StartVolume, musicAudio.EndVolume, elapsedTime / musicAudio.Duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        AudioManager.Instance.musicAudioSource.volume = musicAudio.EndVolume;
    }
}