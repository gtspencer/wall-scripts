using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Random = UnityEngine.Random;

public class ForestHanger : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;
    [SerializeField] private GameObject _source;
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private int _startingTargetIndex;

    [SerializeField] private RoomPathIndicator _indicator;

    private Renderer _renderer;

    private Coroutine _visibilityCoroutine;

    void Start()
    {
        _mainCamera = FirstPersonController.Instance.PlayerCamera;
        
        if (_source == null || _targets.Length == 0 || _mainCamera == null)
        {
            Debug.LogError("Missing required references!");
            return;
        }

        _renderer = _source.GetComponentInChildren<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("_source is missing a Renderer component!");
            return;
        }

        // Spawn at a random target initially
        // MoveToRandomTarget();
        
        // _source.transform.position = _targets[_startingTargetIndex].position;
        // _source.transform.rotation = _targets[_startingTargetIndex].rotation;
        
        _visibilityCoroutine = StartCoroutine(VisibilityCheck());
    }

    private void OnDestroy()
    {
        StopCoroutine(_visibilityCoroutine);
    }

    private IEnumerator VisibilityCheck()
    {
        while (true)
        {
            // Wait until _source is visible
            yield return new WaitUntil(() => _renderer.isVisible);
            _indicator.ToggleValid(true);

            // Wait until _source is NOT visible
            yield return new WaitUntil(() => !_renderer.isVisible);
            _indicator.ToggleValid(false);

            // Move to a new valid target
            MoveToRandomTarget();
        }
    }

    private void MoveToRandomTarget()
    {
        List<Transform> validTargets = new List<Transform>();

        foreach (Transform target in _targets)
        {
            Vector3 viewportPos = _mainCamera.WorldToViewportPoint(target.position);
            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
            {
                validTargets.Add(target);
            }
        }

        if (validTargets.Count > 0)
        {
            Transform newTarget = validTargets[Random.Range(0, validTargets.Count)];
            _source.transform.position = newTarget.position;
            _source.transform.rotation = newTarget.rotation;
        }
    }
}
