using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class RainController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _rain;
    [SerializeField]
    private ParticleSystem _splash;

    [SerializeField] private AudioClip _rainSFX;

    private GameObject _rainAudioSource;

    [Button]
    public void Rain()
    {
        _rainAudioSource = AudioManager.Instance.PlayOneShotSFXAudio(_rainSFX, new Vector3(0, 10, 0), returnWhenDone: false);

        StartCoroutine(StartRain_Coroutine());
    }

    private IEnumerator StartRain_Coroutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        _rain.Play();
        
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        _splash.Play();
    }

    [Button]
    public void StopRain()
    {
        _rainAudioSource.GetComponent<AudioSource>().Stop();

        AudioManager.Instance.ReturnOneShotSFXAudio(_rainAudioSource);
        
        _splash.Stop();
        _rain.Stop();
    }
}
