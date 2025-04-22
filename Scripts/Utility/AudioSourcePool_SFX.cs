using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class SFXAudioSourcePool_SFX : MonoBehaviour
{
    public static SFXAudioSourcePool_SFX Instance;

    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private AudioMixerGroup sfxAudioGroup; 
    
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        pool = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true, initialPoolSize, initialPoolSize * 2);
    }

    private GameObject CreatePooledItem()
    {
        GameObject obj = new GameObject("SFX Audio Source");
        
        var audioSource = obj.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxAudioGroup;
        
        obj.SetActive(false); // Start inactive
        
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        AudioSource source = obj.GetComponent<AudioSource>();
        source.Stop();
        source.clip = null;
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetAudioSource()
    {
        return pool.Get();
    }

    public void ReturnAudioSource(GameObject obj)
    {
        pool.Release(obj);
    }
}