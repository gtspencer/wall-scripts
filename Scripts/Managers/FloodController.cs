using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class FloodController : MonoBehaviour
{
    public static FloodController Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    
    private float _moveDistance = 20f;
    [SerializeField]
    private float _duration = 30f;

    private EnableFloodFog _additionalEffects;

    private void OnEnable()
    {
        _additionalEffects = GetComponent<EnableFloodFog>();
    }

    [Button]
    public void RaiseWater()
    {
        StartCoroutine(RaiseWater_Coroutine());
    }
    
    private IEnumerator RaiseWater_Coroutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * _moveDistance;
        float timeElapsed = 0f;

        while (timeElapsed < _duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / _duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    public void CleanupFlood()
    {
        _additionalEffects.RemoveEffects();
        Destroy(this.gameObject);
    }
}
