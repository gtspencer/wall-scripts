using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{
    [SerializeField]
    private GameObject _black;

    public static Blackout Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    public void ToggleBlackout(bool enabled)
    {
        _black.SetActive(enabled);
    }
}
