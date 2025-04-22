using StarterAssets;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public static TeleportController Instance;
    
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private FirstPersonController fpsc;
    private CharacterController cc;
    
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fpsc = GetComponent<FirstPersonController>();
        cc = GetComponent<CharacterController>();
        
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    public void TeleportToStart()
    {
        TeleportToPoint(_startPosition, true);
    }

    public void TeleportToPoint(Vector3 point, bool resetCameraRotation = false)
    {
        // cc.enabled = false;
        FirstPersonController.Instance.enabled = false;
        FirstPersonController.Instance.GetComponent<CharacterController>().enabled = false;
        
        transform.position = point;
        
        if (resetCameraRotation)
        {
            transform.rotation = _startRotation;
            fpsc.CinemachineCameraTarget.transform.localRotation = Quaternion.identity;
            fpsc._cinemachineTargetPitch = 0;
        }
        
        FirstPersonController.Instance.enabled = true;
        FirstPersonController.Instance.GetComponent<CharacterController>().enabled = true;
    }
}
