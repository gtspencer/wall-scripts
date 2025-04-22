using UnityEngine;

public class VacuumVerseSceneController : MonoBehaviour
{
    public static VacuumVerseSceneController Instance;

    [SerializeField] private Light _directionalLight;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
			
        Instance = this;
    }

    public void DisableLight()
    {
        _directionalLight.enabled = false;
    }
}
