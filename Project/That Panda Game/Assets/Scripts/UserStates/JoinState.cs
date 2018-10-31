﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinState : UserState
{
    private GameScene _scene;
    private Dictionary<CharacterType, string> _characterButtons = new Dictionary<CharacterType, string>();

    public JoinState(User user, SceneManager sceneManager)
        :base(user)
    {
        _scene = sceneManager.GetScene<GameScene>();
        _characterButtons.Add(CharacterType.Panda, "Button1");
        _characterButtons.Add(CharacterType.Lizard, "Button2");
        _characterButtons.Add(CharacterType.Elephant, "Button3");
        _characterButtons.Add(CharacterType.Pig, "Button0");
    }

    public override void Initialize()
    {

    }

    public override void Update()
    {
        //Check for a button press on each button then attemp to assign the corresponding character to the user
        foreach (CharacterType type in _characterButtons.Keys)
        {
            //if (_joystick.WasButtonPressed(_characterButtons[type]) || _joystick.GetId() == 2)
            //{
                if (_scene.AttemptCharacterAssign(type, _user))
                {
                    _user.ChangeState("GameState");
                    return;
                }
            //}
        } 
    }
}
