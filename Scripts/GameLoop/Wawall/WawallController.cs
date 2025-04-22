using System;
using NaughtyAttributes;
using UnityEngine;

public class WawallController : MonoBehaviour
{
    public static WawallController Instance;
    
    [SerializeField] private GameObject _wawallText;

    [SerializeField] private Texture2D _defaultTexture;

    [SerializeField] private WawallGlitchController _glitchController;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    
    [SerializeField] private GameObject _wawallRoot;

    private void OnEnable()
    {
        SetSide1Image(_defaultTexture);
        SetSide2Image(_defaultTexture);
    }

    [Button]
    public void EnableWawall()
    {
        MainWallCanvas.Instance.WriteImmediateSide1("");
        MainWallCanvas.Instance.WriteImmediateSide2("");
        
        _wawallRoot.SetActive(true);
        SetDefaultTextures();
        
        _glitchController.enabled = true;
    }

    public void DisableWawall()
    {
        _wawallRoot.SetActive(false);
    }
    
    public void DisableWawallMainText()
    {
        _wawallText.SetActive(false);
    }

    public void SetDefaultTextures()
    {
        SetSide1Image(_defaultTexture);
        SetSide2Image(_defaultTexture);
    }

    public void SetSide1Image(Texture2D image)
    {
        _glitchController.SetSide1Image(image);
    }
    
    public void SetSide2Image(Texture2D image)
    {
        _glitchController.SetSide2Image(image);
    }

    public void ToggleWawallText(bool enabled)
    {
        _wawallText.SetActive(enabled);
    }

    public void SetManualGlitch(bool manualGlitch)
    {
        _glitchController.SetManualGlitch(manualGlitch);
    }
    
    public void Glitch()
    {
        _glitchController.Glitch();
    }
    
    public void Unglitch()
    {
        _glitchController.Unglitch();
    }
}
