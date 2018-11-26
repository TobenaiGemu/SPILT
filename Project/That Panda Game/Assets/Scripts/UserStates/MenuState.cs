using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : UserState
{
    public MenuState(User user)
        :base(user)
    {
    }

    public override void Initialize()
    {
    }

    public override void Update()
    {
        if (_joystick.WasButtonPressed("Button2"))
        {
            if (!_sceneManager.CheckCurrentScene<MainMenuScene>())
                _sceneManager.ChangeScene<MainMenuScene>();
        }
    }
}
