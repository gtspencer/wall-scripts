using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Roomba : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minRotation;
    [SerializeField] private float maxRotation;
    private Quaternion targetQuaternion;

    [SerializeField] private Transform side1;
    [SerializeField] private Transform side2;
    
    private RoombaStates state;

    [SerializeField] private float _rayLength = .5f;

    private int _layerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        state = RoombaStates.Moving;
        _layerMask = ~LayerMask.GetMask("Player");
    }

    public Vector3 TransformMovement;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * _rayLength, Color.red);
        Debug.DrawLine(side1.position, side1.position + side1.forward * _rayLength, Color.red);
        Debug.DrawLine(side2.position, side2.position + side2.forward * _rayLength, Color.red);
        
        switch (state)
        {
            case RoombaStates.Moving:
                TransformMovement = transform.forward * speed * Time.deltaTime;
                transform.position += TransformMovement;
                
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _rayLength, layerMask: _layerMask))
                {
                    var targetRotation = Random.Range(minRotation, maxRotation);
                    targetQuaternion = Quaternion.Euler(0, transform.eulerAngles.y + targetRotation, 0);
                    state = RoombaStates.Turning;
                } else if (Physics.Raycast(side1.position, side1.forward, out RaycastHit hit1, _rayLength, layerMask: _layerMask))
                {
                    var targetRotation = Random.Range(minRotation, maxRotation);
                    targetQuaternion = Quaternion.Euler(0, transform.eulerAngles.y + targetRotation, 0);
                    state = RoombaStates.Turning;
                } else if (Physics.Raycast(side2.position, side2.forward, out RaycastHit hit2, _rayLength, layerMask: _layerMask))
                {
                    var targetRotation = Random.Range(minRotation, maxRotation);
                    targetQuaternion = Quaternion.Euler(0, transform.eulerAngles.y + targetRotation, 0);
                    state = RoombaStates.Turning;
                }
                
                break;
            case RoombaStates.Turning:
                TransformMovement = Vector3.zero;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime);

                if (Quaternion.Angle(transform.rotation, targetQuaternion) < 0.1f)
                    state = RoombaStates.Moving;
                
                /*
                float rotationStep = targetRotation * Time.deltaTime;
                transform.Rotate(Vector3.up, rotationStep);
                targetRotation -= rotationStep;

                
                
                Debug.LogError("Target: " + targetRotation);
                if (targetRotation <= 0)
                */
                    
                
                break;
        }
    }

    public void OnPlayerStayOnRoomba(FirstPersonController fpc)
    {
        fpc.AddMovementToPlayer(TransformMovement);
    }

    public void OnPlayerExitRoomba(FirstPersonController fpc)
    {
        fpc.AddMovementToPlayer(Vector3.zero);
    }

    private enum RoombaStates
    {
        Moving,
        Turning
    }
}
