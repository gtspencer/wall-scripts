using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class WallPusher : MonoBehaviour
{
    [SerializeField] private Vector3 _pushPoint;
    [SerializeField] private Vector3 _pushDirection;
    [SerializeField] private float _pushStrength;

    [SerializeField] private ParticleSystem _vfx;
    [SerializeField] private AudioClip _fallClip;

    private Rigidbody rb;

    [SerializeField] private float _minPitch = 0.75f;
    [SerializeField] private float _maxPitch = 0.9f;
    
    public void Push()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.AddForceAtPosition(_pushDirection * _pushStrength, _pushPoint);
    }

    [Button]
    public void PlayFallEffect()
    {
        AudioManager.Instance.PlayOneShotSFXAudio(_fallClip, transform.position, volume: 0.75f, pitch: UnityEngine.Random.Range(_minPitch, _maxPitch));
        
        _vfx.Play();

        StartCoroutine(DisableRigidbody());
    }

    private IEnumerator DisableRigidbody()
    {
        yield return new WaitForSeconds(0.1f);

        Destroy(rb);
    }
}
