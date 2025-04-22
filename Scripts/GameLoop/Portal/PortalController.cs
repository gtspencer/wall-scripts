using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public static PortalController Instance;

    public Action OnPlayerTeleportFromEntrance;
    
    [SerializeField] private GameObject portalEntranceObject;
    [SerializeField] private GameObject portalCamera;

    private Collider portalCollider;
    private PortalTeleporter entranceTeleporter;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        entranceTeleporter = GetComponentInChildren<PortalTeleporter>();
        entranceTeleporter.OnPlayerTeleport += PlayerTeleportedFromEntrance;
        portalEntranceObject.SetActive(false);
        portalCamera.SetActive(false);

        portalCollider = portalEntranceObject.GetComponent<Collider>();
        portalCollider.enabled = false;
    }

    [Button]
    public void EnablePortalImmediate()
    {
        portalEntranceObject.SetActive(true);
        portalCamera.SetActive(true);

        TogglePortalCollider(true);
    }

    public void DisablePortalImmediate()
    {
        portalEntranceObject.SetActive(false);
        portalCamera.SetActive(false);

        TogglePortalCollider(false);
    }

    private void PlayerTeleportedFromEntrance()
    {
        OnPlayerTeleportFromEntrance?.Invoke();

        DisablePortalImmediate();
    }

    private void TogglePortalCollider(bool enabled)
    {
        portalCollider.enabled = enabled;
    }

    private void OnDestroy()
    {
        entranceTeleporter.OnPlayerTeleport -= PlayerTeleportedFromEntrance;
        Destroy(portalCamera);
    }
}
