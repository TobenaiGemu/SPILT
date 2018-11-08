using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectState : UserState
{
    private SceneManager _sceneManager;

    public CharacterSelectState(User user, SceneManager sceneManager)
        :base(user)
    {
        _sceneManager = sceneManager;
    }

    public override void Update()
    {
        if (_joystick.WasButtonPressed("Button2"))
        {
            _sceneManager.ChangeScene<JoinGameScene>();
        }
    }
}
