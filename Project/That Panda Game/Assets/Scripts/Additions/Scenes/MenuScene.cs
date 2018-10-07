using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScene : Scene
{
    private GameObject _mainMenuPanel;
    private GameObject _initButton;

    private GameObject _infoPanel;
    private GameObject _gameModePanel;
    private GameObject _settingsPanel;

    public MenuScene(SceneManager sceneManager)
        :base(sceneManager)
    {
        _mainMenuPanel = GameObject.Find("Canvas").transform.Find("MainMenuPanel").gameObject;
        _initButton = _mainMenuPanel.transform.Find("Start").gameObject;
        _infoPanel = GameObject.Find("Canvas").transform.Find("InfoPanel").gameObject;
        _gameModePanel = GameObject.Find("Canvas").transform.Find("GameModePanel").gameObject;
        _settingsPanel = GameObject.Find("Canvas").transform.Find("SettingsPanel").gameObject;
    }

    public override void Initialize()
    {
        foreach (User user in SceneManager.Users)
            user.ChangeState("MenuState");
        _mainMenuPanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = _initButton;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_initButton);
    }

    public override void Cleanup()
    {
        _infoPanel.SetActive(false);
        _gameModePanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    public override void Update()
    {
        if (_uiManager.IsHovering("Info"))
            _infoPanel.SetActive(true);
        else
            _infoPanel.SetActive(false);

        if (_uiManager.IsHovering("Start"))
            _gameModePanel.SetActive(true);
        else
            _gameModePanel.SetActive(false);
        if (_uiManager.IsHovering("Settings"))
            _settingsPanel.SetActive(true);
        else
            _settingsPanel.SetActive(false);
    }
}
