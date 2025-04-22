using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SuckerUpper : MonoBehaviour
{
    [SerializeField] private AudioClip _suckClip;

    public UnityEvent OnSuckUp = new UnityEvent();

    private bool _sucked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        if (_sucked) return;

        Suck();
    }

    private void Suck()
    {
        _sucked = true;
        AudioManager.Instance.PlayOneShotSFXAudio(_suckClip, transform.position);
        Blackout.Instance.ToggleBlackout(true);
        
        OnSuckUp.Invoke();
    }
}
