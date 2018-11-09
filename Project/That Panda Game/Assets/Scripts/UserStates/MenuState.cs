using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : UserState
{
    private MainMenuScene _scene;
    private SceneManager _sceneManager;

    public MenuState(User user, SceneManager sceneManager)
        :base(user)
    {
        _scene = sceneManager.GetScene<MainMenuScene>();
        _sceneManager = sceneManager;
    }

    public override void Initialize()
    {
    }

    public override void Update()
    {
        if (_joystick.WasButtonPressed("Button2"))
        {
            _sceneManager.ChangeScene<MainMenuScene>();
        }
    }
}
