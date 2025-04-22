using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

    [SerializeField] private Transform playerCamera;
    [SerializeField] private Camera portalCamera;
    [SerializeField] private Material portalMaterialB;
    [SerializeField] private Transform portalExit;
    [SerializeField] private Transform mainRoomPortalEntrance;

    public void ChangePortalDestination(PortalDestination newDestination)
    {
        portalExit = newDestination.portalDestination;
    }

    private void Start()
    {
        if (portalCamera.targetTexture != null)
        {
            portalCamera.targetTexture.Release();
        }

        portalCamera.targetTexture = new RenderTexture(Screen.width / 2, Screen.height / 2, 24);
        portalMaterialB.mainTexture = portalCamera.targetTexture;
    }

    private void OnDestroy()
    {
        if (portalCamera.targetTexture != null)
        {
            portalCamera.targetTexture.Release();
        }
    }

    // Update is called once per frame
    void Update () {
        Vector3 playerOffsetFromPortal = playerCamera.position - mainRoomPortalEntrance.position;
        transform.position = portalExit.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portalExit.rotation, mainRoomPortalEntrance.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.down);

        var rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        transform.rotation = rotation;
    }
}