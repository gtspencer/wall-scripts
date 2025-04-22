using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceLight : MonoBehaviour
{
    public float rotationSpeed = 30f; // Base rotation speed
    public float angleRange = 30f; // Maximum angle deviation from the center

    private float timeOffset;
    private Quaternion initialRotation;

    void Start()
    {
        timeOffset = Random.Range(0f, 100f); // Offset to make multiple lights desynchronized
        initialRotation = transform.rotation; // Store initial rotation as a quaternion
    }

    void Update()
    {
        float time = Time.time + timeOffset;
        
        // Calculate oscillating angles
        float xRotation = Mathf.Sin(time * rotationSpeed * 0.1f) * angleRange;
        float zRotation = Mathf.Cos(time * rotationSpeed * 0.1f) * angleRange;
        
        // Create rotation deltas
        Quaternion xQuat = Quaternion.AngleAxis(xRotation, transform.right);
        Quaternion zQuat = Quaternion.AngleAxis(zRotation, transform.forward);
        
        // Apply rotation while keeping the light's original orientation
        transform.rotation = initialRotation * xQuat * Quaternion.Inverse(zQuat);
    }
}
