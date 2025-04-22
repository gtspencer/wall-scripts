using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudienceMannequin : MonoBehaviour
{
    private GameObject _tomatoPrefab;
    private GameObject _rosePrefab;

    [SerializeField] private float _minThrowForce = 8f;
    [SerializeField] private float _maxThrowForce = 15f;
    
    [SerializeField] private Vector2 _pitchRange = new Vector2(-10f, 10f); // X
    [SerializeField] private Vector2 _yawRange = new Vector2(-10f, 10f);   // Y
    [SerializeField] private Vector2 _rollRange = new Vector2(-10f, 10f);  // Z

    [SerializeField] private float _minWaitThrowTime = 1f;
    [SerializeField] private float _maxWaitThrowTime = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tomatoPrefab = Resources.Load<GameObject>("Prefabs/Tomato");
        _rosePrefab = Resources.Load<GameObject>("Prefabs/Rose");
    }

    public void ThrowRose()
    {
        StartCoroutine(ThrowRoses_Coroutine());
    }

    private IEnumerator ThrowRoses_Coroutine()
    {
        yield return WaitToThrow_Coroutine(_rosePrefab);
        yield return WaitToThrow_Coroutine(_rosePrefab);
    }

    public void ThrowTomato(Action onDoneThrowing)
    {
        StartCoroutine(WaitToThrow_Coroutine(_tomatoPrefab, onDoneThrowing));
    }

    [Button]
    private void Throw(GameObject throwablePrefab)
    {
        var throwable = GameObject.Instantiate(throwablePrefab, transform.position, Quaternion.identity);
        
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Thrown object does not have a Rigidbody!");
            return;
        }
        
        Vector3 baseDirection = (transform.forward + transform.up).normalized;
        
        float pitch = Random.Range(_pitchRange.x, _pitchRange.y);
        float yaw = Random.Range(_yawRange.x, _yawRange.y);
        float roll = Random.Range(_rollRange.x, _rollRange.y);
        
        Quaternion offsetRotation = Quaternion.Euler(pitch, yaw, roll);
        
        Vector3 finalDirection = (offsetRotation * baseDirection).normalized;

        rb.AddForce(finalDirection * UnityEngine.Random.Range(_minThrowForce, _maxThrowForce), ForceMode.Impulse);
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * Random.Range(3f, 7f);

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }
    
    private IEnumerator WaitToThrow_Coroutine(GameObject throwable, Action onDoneThrowing = null)
    {
        yield return new WaitForSeconds(Random.Range(_minWaitThrowTime, _maxWaitThrowTime));
        Throw(throwable);
        // yield return new WaitForSeconds(Random.Range(_minWaitThrowTime, _maxWaitThrowTime));
        // Throw();
        if (onDoneThrowing != null)
            onDoneThrowing.Invoke();
    }
}
