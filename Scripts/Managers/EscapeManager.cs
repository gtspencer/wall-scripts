using System;
using UnityEngine;

public class EscapeManager : MonoBehaviour
{
    public static EscapeManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _reticle;

    private bool _previousReticleState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputRepository.Instance.OnEscape += OnEscape;
    }

    private void OnEscape()
    {
        if (_menu.activeSelf)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        _previousReticleState = _reticle.activeSelf;
        _reticle.SetActive(false);
            
        _menu.SetActive(true);
            
        InputRepository.Instance.ChangeInputType(InputRepository.InputType.UI);
            
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        _menu.SetActive(false);
        _reticle.SetActive(_previousReticleState);
            
        InputRepository.Instance.ChangeInputType(InputRepository.InputType.Movement);
            
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        InputRepository.Instance.OnEscape -= OnEscape;
    }
}
