using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsState : UserState
{
    private SceneManager _sceneManager;

	public OptionsState(User user, SceneManager sceneManager)
        :base(user)
    {
        _sceneManager = sceneManager;
    }

    public override void Update()
    {
        if (_joystick.WasButtonPressed("Button2"))
        {
            _sceneManager.GetScene<OptionsScene>().GotoLeftButtons();
        }
    }
}
