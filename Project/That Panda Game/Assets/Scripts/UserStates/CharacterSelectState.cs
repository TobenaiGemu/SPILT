using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectState : UserState
{
    public CharacterSelectState(User user)
        :base(user)
    {
    }

    public override void Update()
    {
        if (_joystick.WasButtonPressed("Button2"))
        {
            _sceneManager.ChangeScene<JoinGameScene>();
        }
    }
}
