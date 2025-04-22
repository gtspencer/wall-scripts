using System;
using System.Collections;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class HenryController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _slideCam;
    private FirstPersonController _fpc;

    [SerializeField] private Transform _teleportLocation;

    [SerializeField] private float _rideTime = 10f;
    [SerializeField] private SplineAnimate _splineAnimate;

    [SerializeField] private GameObject _teleportHole;
    [SerializeField] private AudioClip _plasticSlideSource;

    private void OnEnable()
    {
        _fpc = FirstPersonController.Instance;
    }

    public void RideHenry()
    {
        StartCoroutine(RideHenry_Coroutine());
    }

    private IEnumerator RideHenry_Coroutine()
    {
        _slideCam.Priority = 25;
        _fpc.enabled = false;
        _splineAnimate.Play();

        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.PlayOneShotSFXAudio(_plasticSlideSource, _fpc.transform.position);
        
        yield return new WaitForSeconds(_rideTime - 1.5f);

        VacuumVerseController.Instance.LeaveVacuumVerse();
        _teleportHole.SetActive(true);
        TeleportController.Instance.TeleportToPoint(_teleportLocation.position);
        _fpc.enabled = true;
        _slideCam.Priority = 0;
    }
}
