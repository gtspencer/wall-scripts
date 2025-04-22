using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class PortalDestination
{
    public Transform portalDestination;
    public PortalManager.PortalDestinations portalType;
}

public class PortalManager : MonoBehaviour
{
    #region References

    [SerializeField] private PortalCamera _portalCamera;
    [SerializeField] private PortalTeleporter _portalTeleporter;
    

    #endregion
    
    public enum PortalDestinations
    {
        TestRoom,
        RoomPath,
    }

    [SerializeField]
    private PortalDestination[] _portalDestinationsRaw;

    private Dictionary<PortalDestinations, PortalDestination> _portalDestinations =
        new Dictionary<PortalDestinations, PortalDestination>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var portalDestination in _portalDestinationsRaw)
        {
            _portalDestinations.Add(portalDestination.portalType, portalDestination);
        }
    }

    public void UpdateDestination(PortalDestinations destinationId)
    {
        if (_portalDestinations.TryGetValue(destinationId, out PortalDestination destination))
        {
            _portalCamera.ChangePortalDestination(destination);
            _portalTeleporter.ChangeReceiver(destination.portalDestination);

            return;
        }
        
        Debug.LogError($"Failed to grab portal destination {destinationId}");
    }

    [Button]
    public void TestPortalWallHall()
    {
        UpdateDestination(PortalDestinations.RoomPath);
    }

    public void SetPortal_RoomPath()
    {
        UpdateDestination(PortalDestinations.RoomPath);
    }
}
