using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotation;
    [SerializeField]
    private float _rotationSpeed = 100f;

    void FixedUpdate()
    {
        Vector3 rotation = _rotation * _rotationSpeed * Time.fixedDeltaTime;
        
        transform.Rotate(rotation);
    }
}
