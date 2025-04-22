using UnityEngine;

public class VacuumVerseController : MonoBehaviour
{
    public static VacuumVerseController Instance;

    [SerializeField] private Material _verseSkybox;
    [SerializeField] private Material _blackSkybox;

    [SerializeField] private GameObject _teleportTubeMesh;
    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);

        Instance = this;
    }

    public void EnterTheVacuumVerse()
    {
        RenderSettings.skybox = _verseSkybox;
        
        // Camera.main.b
    }

    public void LeaveVacuumVerse()
    {
        VacuumVerseSceneController.Instance.DisableLight();
        _teleportTubeMesh.SetActive(true);
        RenderSettings.skybox = _blackSkybox;
    }
}
