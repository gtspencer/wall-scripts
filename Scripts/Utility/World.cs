using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World Instance;

    public Transform WallMesh;
    public Transform WallController;
    
    [Header("Teleport Points")]
    public Transform VacuumVerseTeleportLocation;
    public Transform CorpoRoomTeleportLocation;
    public Transform CorpoRoomFromVacuumVerseTeleportLocation;
    public Transform RoomPathTeleportLocation;
    public Transform CreationRoomTeleportLocation;
    
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        Instance = this;
    }
}
