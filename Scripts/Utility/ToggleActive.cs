using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    private void Start()
    {
        if (_transform == null)
        {
            Debug.LogWarning($"Transform is not set in ToggleActive, defaulting to self.");
            _transform = this.transform;
        }
    }

    public void Toggle(bool enabled)
    {
        _transform.gameObject.SetActive(enabled);
    }
}
