using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AmbientAudio
{
    public AmbientAudioTypes AmbientType;
    public AudioClip Clip;
    public float StartVolume;
    public float EndVolume;
    public float Duration;
}

public enum AmbientAudioTypes
{
    Ending,
}

public class AmbientAudioManager : MonoBehaviour
{
    public static AmbientAudioManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        foreach (var ambientAudio in _ambientAudiosRaw)
            _ambientAudios.Add(ambientAudio.AmbientType, ambientAudio);
    }

    [SerializeField]
    private AmbientAudio[] _ambientAudiosRaw;

    private Dictionary<AmbientAudioTypes, AmbientAudio>
        _ambientAudios = new Dictionary<AmbientAudioTypes, AmbientAudio>();

    public void PlayEndingAmbientAudio()
    {
        FadeInAmbient(AmbientAudioTypes.Ending);
    }

    public void FadeInAmbient(AmbientAudioTypes audioType)
    {
        StartCoroutine(FadeInAmbient_Coroutine(audioType));
    }

    private IEnumerator FadeInAmbient_Coroutine(AmbientAudioTypes audioType)
    {
        if (!_ambientAudios.ContainsKey(audioType)) yield return null;

        var ambientAudio = _ambientAudios[audioType];

        AudioManager.Instance.ambientAudioSource.clip = ambientAudio.Clip;
        AudioManager.Instance.ambientAudioSource.volume = ambientAudio.StartVolume;
        AudioManager.Instance.ambientAudioSource.Play();
        
        var elapsedTime = 0f;
        
        while (elapsedTime < ambientAudio.Duration)
        {
            AudioManager.Instance.ambientAudioSource.volume = Mathf.Lerp(ambientAudio.StartVolume, ambientAudio.EndVolume, elapsedTime / ambientAudio.Duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        AudioManager.Instance.ambientAudioSource.volume = ambientAudio.EndVolume;
    }
}
