using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinState : UserState
{
    private GameScene _scene;
    private Dictionary<CharacterType, string> _characterButtons = new Dictionary<CharacterType, string>();

    public JoinState(User user, GameScene scene)
        :base(user)
    {
        _scene = scene;
    }

    public override void Initialize()
    {
        Debug.Log("JoinState");
        _characterButtons.Add(CharacterType.Panda, "Button1");
        _characterButtons.Add(CharacterType.Lizard, "Button2");
        _characterButtons.Add(CharacterType.Elephant, "Button3");
        _characterButtons.Add(CharacterType.Pig, "Button0");
    }

    public override void Update()
    {
        //Check for a button press on each button then attemp to assign the corresponding character to the user
        foreach (CharacterType type in _characterButtons.Keys)
        {
            if (_joystick.WasButtonPressed(_characterButtons[type]))
            {
                if (_scene.AttemptCharacterAssign(type, _user))
                {
                    _user.ChangeState("GameState");
                }
            }
        } 
    }
}
