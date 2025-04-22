using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyer : MonoBehaviour
{
    [SerializeField] private Transform wallPieces;
    [SerializeField] private Transform mainWall;
    private BuildingPiece[] _buildingPieces;

    [SerializeField] private Transform buildingCenter;
    [SerializeField] private float timeToRebuild = 3f;
    [Header("Collision and Force Settings")]
    [SerializeField] private bool useCollisionForces = false;
    [Header("Only used if UseCollisionForce is FALSE")] 
    [SerializeField] private float destructionMinForce = 3f;
    [SerializeField] private float destructionMaxForce = 7f;

    private float distanceToCenter;
    // Start is called before the first frame update
    void Start()
    {
        var colliders = wallPieces.GetComponentsInChildren<Collider>();
        _buildingPieces = new BuildingPiece[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            var c = colliders[i];
            
            var go = c.gameObject;
            
            _buildingPieces[i] = new BuildingPiece(go, c);
        }

        wallPieces.gameObject.SetActive(false);
    }
    
    public void DestroyBuilding()
    {
        wallPieces.gameObject.SetActive(true);
        mainWall.gameObject.SetActive(false);
        
        foreach (BuildingPiece b in _buildingPieces)
        {
            b.EnableDestruction(buildingCenter, destructionMinForce, destructionMaxForce, useCollisionForces);
        }
    }

    public bool testDestroy;
    public bool testRebuild;
    private void Update()
    {
        if (testDestroy)
        {
            testDestroy = false;
            DestroyBuilding();
        }

        if (testRebuild)
        {
            testRebuild = false;
            EnableRebuild();
        }

        if (shouldRebuild)
        {
            timeToRebuild_Counter += Time.deltaTime;

            var isDone = MovePiecesBack(timeToRebuild_Counter / timeToRebuild);

            if (isDone)
            {
                Debug.LogError("Done rebuilding");
                shouldRebuild = false;
            }
        }
    }

    private bool shouldRebuild;
    
    private float timeToRebuild_Counter = 0f;

    public void EnableRebuild()
    {
        foreach (BuildingPiece b in _buildingPieces)
        {
            b.EnableRebuild();
        }

        timeToRebuild_Counter = 0;
        shouldRebuild = true;
    }
    
    public bool MovePiecesBack(float progress)
    {
        // var distance = (buildingCenter.position - playerPosition).magnitude;

        // var progress = 1 - (distance / distanceToCenter);

        Debug.Log("Progress " + progress);
        int donePieces = 0;
        foreach (BuildingPiece b in _buildingPieces)
        {
            bool isPieceMoving = b.Move(progress);
            
            if (!isPieceMoving) donePieces++;
        }

        if (donePieces >= _buildingPieces.Length)
        {
            foreach (BuildingPiece b in _buildingPieces)
            {
                b.ResetPiece();
            }
            
            wallPieces.gameObject.SetActive(false);
            mainWall.gameObject.SetActive(true);

            return true;
        }

        return false;
    }

    public void MovePiecesBack_PlayerPosition(Vector3 playerPosition)
    {
        var distance = (buildingCenter.position - playerPosition).magnitude;

        var progress = 1 - (distance / distanceToCenter);

        int donePieces = 0;
        foreach (BuildingPiece b in _buildingPieces)
        {
            bool isPieceMoving = b.Move(progress);
            
            if (!isPieceMoving) donePieces++;
        }

        if (donePieces >= _buildingPieces.Length)
        {
            foreach (BuildingPiece b in _buildingPieces)
            {
                b.ResetPiece();
            }
            
            // wallPieces.gameObject.SetActive(false);
            // mainWall.gameObject.SetActive(true);
        }
    }

    public void PlayerEntered(Transform player)
    {
        distanceToCenter = (buildingCenter.position - player.position).magnitude;
        EnableRebuild();
    }
}

public class BuildingPiece
{
    public GameObject _gameObject;
    public Transform _transform;
    public Rigidbody rb;
    public Collider c;

    private Vector3 startingPos;
    private Quaternion startingRotation;

    public BuildingPiece(GameObject gameObject, Collider c)
    {
        this._gameObject = gameObject;
        this.c = c;
        this._transform = _gameObject.transform;
        
        this.rb = _gameObject.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = _gameObject.AddComponent<Rigidbody>();
        }
        
        rb.useGravity = false;
        rb.isKinematic = true;
        
        startingPos = _gameObject.transform.position;
        startingRotation = _gameObject.transform.rotation;
    }

    public bool Move(float progress)
    {
        if (Vector3.Distance(_transform.position, startingPos) <= .001f) return false;
        
        _transform.position = Vector3.Lerp(destroyedRestingPosition, startingPos, progress);
        _transform.rotation = Quaternion.Lerp(destroyedRestingRotation, startingRotation, progress);

        return true;
    }

    public void ResetPiece()
    {
        c.enabled = true;
        
        if (c is MeshCollider mc)
        {
            mc.convex = false;
        }
        
        rb.isKinematic = true;
    }

    private Vector3 destroyedRestingPosition;
    private Quaternion destroyedRestingRotation;
    public void EnableRebuild()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        
        if (c is MeshCollider mc)
        {
            mc.convex = false;
        }

        c.enabled = false;

        destroyedRestingPosition = _transform.position;
        destroyedRestingRotation = _transform.rotation;
    }

    public void EnableDestruction(Transform buildingCenter, float minForce, float maxForce, bool useCollisionForces = false)
    {
        if (c is MeshCollider mc)
        {
            mc.convex = true;
        }
        
        if (useCollisionForces)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            // c.enabled = false;
            
            var force = buildingCenter.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(-1f, 1f));
            force.Normalize();
            
            Debug.Log("Destruction vector " + force);
        
            rb.AddForce(force * UnityEngine.Random.Range(minForce, maxForce), ForceMode.Impulse);

            _gameObject.AddComponent<BasicHoldable>();
        }
    }
}
