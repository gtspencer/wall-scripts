using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Action OnPlayerTeleport;
    [SerializeField] private Transform player;

    [SerializeField] private Transform receiver;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var cc = other.GetComponent<CharacterController>();
        // var fpsc = other.GetComponent<FirstPersonController>();

        cc.enabled = false;
        // fpsc.enabled = false;
        
        var portalToPlayer = player.position - transform.position;
        
        var rotationDiff = Quaternion.Angle(transform.rotation, receiver.rotation);
            
        player.Rotate(Vector3.up, rotationDiff);

        Vector3 positionOffset = Quaternion.Euler(0, rotationDiff, 0) * portalToPlayer;
        var position = receiver.position + positionOffset;
        player.position = position;
        
        cc.enabled = true;
        // fpsc.enabled = true;
        
        OnPlayerTeleport?.Invoke();
    }

    public void ChangeReceiver(Transform newReceiver)
    {
        receiver = newReceiver;
    }
}
