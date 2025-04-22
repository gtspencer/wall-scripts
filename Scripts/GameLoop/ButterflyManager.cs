using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ButterflyManager : MonoBehaviour
{
    [SerializeField] private List<Butterfly> butterflies = new List<Butterfly>();
    [SerializeField] private float minWaitTime = .3f;
    [SerializeField] private float maxWaitTime = 1.5f;
    
    [Button]
    public void LetThemFly()
    {
        StartCoroutine(LetThemFly_Coroutine());
    }
    
    private IEnumerator LetThemFly_Coroutine()
    {
        foreach (Butterfly b in butterflies)
        {
            b.gameObject.SetActive(true);
            var waitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
