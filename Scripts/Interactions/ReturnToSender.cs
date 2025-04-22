using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ReturnToSender : MonoBehaviour
{
    private Vector3 _startingPosition;
    [SerializeField] private float maxDistanceFromStart = 50f;
    [SerializeField] private float distanceCheckInterval_sec = 5f;

    [SerializeField] private bool overrideRespawnPosition;

    [SerializeField] private bool destroyInstead;

    [EnableIf("overrideRespawnPosition")]
    [SerializeField] private Vector3 overriddenRespawnPosition;

    private bool _checkPosition = true;

    private void Start()
    {
        _startingPosition = transform.position;

        StartCoroutine(CheckDistance());
    }

    private IEnumerator CheckDistance()
    {
        while (_checkPosition)
        {
            if (Vector3.Distance(_startingPosition, transform.position) >= maxDistanceFromStart)
            {
                if (destroyInstead)
                {
                    Debug.LogWarning($"RETURN TO SENDER: {gameObject.name} got lost, returning to God");
                    _checkPosition = false;
                    Destroy(gameObject);
                    yield return null;
                }
                
                Debug.LogWarning($"RETURN TO SENDER: Returning {gameObject.name} to sender");
                transform.position = _startingPosition;
            }
            
            yield return new WaitForSeconds(distanceCheckInterval_sec);
        }
    }
}
