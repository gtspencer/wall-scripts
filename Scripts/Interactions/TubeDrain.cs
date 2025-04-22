using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class TubeDrain : MonoBehaviour
{
    [SerializeField] private Holdable _plug;
    [SerializeField] private Transform _water;
    [SerializeField] private float _drainDuration;
    [SerializeField] private float _moveDistance;
    [SerializeField] private float _ragdollAfterSec;

    [SerializeField] private Renderer _indicator;
    [SerializeField] private Material _offColor;

    private Ragdoll _ragdoll;

    [SerializeField]
    private AudioSource _drainSource;

    [SerializeField] private AudioClip _glassHit;

    private bool _drained;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (_drained) return;
        _ragdoll = GetComponentInChildren<Ragdoll>();

        _plug.OnHeldCallback += Drain;
    }

    private void OnDisable()
    {
        if (_drained) return;
        _plug.OnHeldCallback -= Drain;
    }
    
    public void Drain(Holdable holdable)
    {
        _drained = true;
        _plug.OnHeldCallback -= Drain;
        
        AchievementManager.AchievementEventRepository.OnUnPlug.Invoke();
        _indicator.material = _offColor;
        StartCoroutine(DrainAudio_Coroutine());
        StartCoroutine(Drain_Coroutine());
        StartCoroutine(Ragdoll_Coroutine());
    }

    private IEnumerator Drain_Coroutine()
    {
        var elapsed = 0f;

        var starting = _water.position;
        var ending = starting - new Vector3(0, _moveDistance);

        while (elapsed < _drainDuration)
        {
            elapsed += Time.deltaTime;

            var t = elapsed / _drainDuration;
            _water.position = Vector3.Lerp(starting, ending, t);

            yield return null;
        }
    }

    private IEnumerator Ragdoll_Coroutine()
    {
        yield return new WaitForSeconds(_ragdollAfterSec);
        _ragdoll.DoTheDoll();
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.PlayOneShotSFXAudio(_glassHit, transform.position);
    }

    private IEnumerator DrainAudio_Coroutine()
    {
        _drainSource.Play();

        yield return new WaitForSeconds(_drainDuration / 2);

        var volume = _drainSource.volume;

        var fadeDuration = _drainDuration / 2;
        
        var elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            var t = 1 - (elapsed / fadeDuration);

            _drainSource.volume = volume * t;

            yield return null;
        }
    }
}
