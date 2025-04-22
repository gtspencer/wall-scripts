using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class StormManager : MonoBehaviour
{
    private RainController _rain;

    private LightningController[] _lightnings;

    [SerializeField] private GameObject[] _clouds;

    [SerializeField] private float _minLightningTime = 3;
    [SerializeField] private float _maxLightningTime = 7;

    private bool _doLightning;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _rain = GetComponentInChildren<RainController>();
        _lightnings = GetComponentsInChildren<LightningController>();
    }

    [Button]
    public void StartStorm()
    {
        foreach (var cloud in _clouds)
            cloud.SetActive(true);
        
        _rain.Rain();

        _doLightning = true;
        StartCoroutine(RandomLightning());
    }

    private IEnumerator RandomLightning()
    {
        while (_doLightning)
        {
            var waitTime = Random.Range(_minLightningTime, _maxLightningTime);

            yield return new WaitForSeconds(waitTime);
            
            // in case lightning gets turned off while waiting in the coroutine (which is likely tbh)
            if (_doLightning)
                _lightnings[Random.Range(0, _lightnings.Length - 1)].PlayEffect();
        }
    }
    
    public void StopStorm()
    {
        foreach (var cloud in _clouds)
            cloud.SetActive(false);
        
        _rain.StopRain();

        _doLightning = false;
    }

    private void Destroy()
    {
        Destroy(_rain.gameObject);
        foreach (var lightning in _lightnings)
            Destroy(lightning.gameObject);
    }
}
