using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyEvent : ScriptedEvent
{
    [SerializeField] private ButterflyManager _butterflies;
    [SerializeField] private float _butterflyFlyTime = 25f;
    protected override void OnStart()
    {
        _butterflies.gameObject.SetActive(true);
        _butterflies.LetThemFly();

        StartCoroutine(WaitForButterflies());
    }

    private IEnumerator WaitForButterflies()
    {
        yield return new WaitForSeconds(_butterflyFlyTime);
        
        Complete();
    }

    protected override void OnUpdate()
    {
    }

    protected override void OnEnd()
    {
        Destroy(_butterflies.gameObject);
    }

    protected override void OnEnteredSide1()
    {
    }

    protected override void OnEnteredSide2()
    {
    }
}
