using NaughtyAttributes;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] _rbs;

    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in _rbs)
            rb.isKinematic = true;

        _animator = GetComponent<Animator>();
    }

    [Button]
    public void DoTheDoll()
    {
        _animator.enabled = false;
        foreach (var rb in _rbs)
            rb.isKinematic = false;
    }
}
