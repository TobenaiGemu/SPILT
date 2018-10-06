using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScene : Scene
{
    private GameObject _mainMenuPanel;
    private GameObject _initButton;

    public MenuScene(SceneManager sceneManager)
        :base(sceneManager)
    {
        _mainMenuPanel = GameObject.Find("Canvas").transform.Find("MainMenuPanel").gameObject;
        _initButton = _mainMenuPanel.transform.Find("Start").gameObject;
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

    public override void Update()
    {
        
    }
}
