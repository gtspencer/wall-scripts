using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSwitcher : MonoBehaviour
{
    public static CharacterSwitcher Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    [Header("First Person References")]
    [SerializeField]
    private FirstPersonController firstPersonController;
    [SerializeField]
    private CinemachineCamera playerCinemachine;
    
    [Header("Wall References")]
    [SerializeField]
    private WallCameraController wallCameraController;
    [SerializeField]
    private CinemachineCamera wallCinemachine;
    private Collider wallCameraCollider;
    
    [Header("Events")]
    public UnityEvent OnSwitchToFirstPerson = new UnityEvent();
    public UnityEvent OnSwitchToWall = new UnityEvent();

    private void Start()
    {
        wallCameraCollider = wallCinemachine.GetComponentInChildren<Collider>();
    }

    public void SetToFirst()
    {
        playerCinemachine.Priority = 20;
        wallCinemachine.Priority = 10;

        // thirdPersonController.gameObject.SetActive(false);
        wallCameraController.enabled = false;
        wallCameraController.gameObject.layer = LayerMask.NameToLayer("Default");
        wallCameraCollider.enabled = false;
        
        firstPersonController.gameObject.SetActive(true);
        firstPersonController.enabled = true;
        
        OnSwitchToFirstPerson.Invoke();
    }

    public void SetToWall()
    {
        playerCinemachine.Priority = 10;
        wallCinemachine.Priority = 20;
        
        firstPersonController.enabled = false;
        firstPersonController.gameObject.SetActive(false);
        
        wallCameraController.enabled = true;
        wallCameraCollider.enabled = true;
        wallCameraCollider.gameObject.layer = LayerMask.NameToLayer("Player");
        // thirdPersonController.gameObject.SetActive(true);
        
        OnSwitchToWall.Invoke();
    }
}
