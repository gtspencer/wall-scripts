using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    public static Mainmenu Instance;

    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _optionsButton;
    [SerializeField] private GameObject _quitButton;

    [SerializeField] private GameObject _optionsMenu;
    
    // only enable this once the player has started the game
    [SerializeField] private GameObject _escapeManager;

    [SerializeField] private RawImage _bgImage;
    [SerializeField] private Texture2D _wallImage;
    [SerializeField] private Texture2D _settingsImage;
    
    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);

        Instance = this;
    }
    
    [SerializeField] private GameObject input;

    private bool _firstGo = true;

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        OnQuitOptions();
    }

    public void OnPressStart()
    {
        if (_firstGo)
        {
            _firstGo = false;
            InputRepository.Instance.EnableInput();
            CharacterSwitcher.Instance.SetToFirst();

            _bgImage.gameObject.SetActive(true);
            _startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
            _escapeManager.SetActive(true);
        }
        else
        {
            // this just turns into a resume button after the first start press
            EscapeManager.Instance.CloseMenu();
        }
        
        gameObject.SetActive(false);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }

    public void OnPressOptions()
    {
        _startButton.SetActive(false);
        _optionsButton.SetActive(false);
        _quitButton.SetActive(false);

        if (!_firstGo)
            _bgImage.texture = _settingsImage;
        _optionsMenu.SetActive(true);
    }

    public void OnQuitOptions()
    {
        _optionsMenu.SetActive(false);
        if (!_firstGo)
            _bgImage.texture = _wallImage;
        
        _startButton.SetActive(true);
        _optionsButton.SetActive(true);
        _quitButton.SetActive(true);
    }

    public void EnableMenu()
    {
        _startButton.SetActive(true);
        _quitButton.SetActive(true);
        _optionsButton.SetActive(true);
    }
}
