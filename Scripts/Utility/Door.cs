using System;
using UnityEngine;
using System.Collections;
using NaughtyAttributes;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float moveDistance = 1f;
    [SerializeField]
    public float duration = 5f;

    private Coroutine _openCoroutine;

    private Vector3 _staringPosition;
    private Vector3 _endPosition;

    private bool _opened;
    private bool _closed;
    private void Start()
    {
        _staringPosition = transform.localPosition;
        _endPosition = _staringPosition + new Vector3(0, moveDistance, 0);
    }

    [Button]
    public void Open()
    {
        if (_opened) return;
        
        _opened = true;
        _openCoroutine = StartCoroutine(Open_Coroutine(_endPosition));
    }

    [Button]
    public void Close()
    {
        if (_closed) return;
        
        _closed = true;
        
        if (_openCoroutine != null)
        {
            StopCoroutine(_openCoroutine);
            _openCoroutine = null;
        }
        
        StartCoroutine(Open_Coroutine(_staringPosition));
    }

    private IEnumerator Open_Coroutine(Vector3 movePosition)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = movePosition; // startPosition + moveDirection * moveDistance;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}