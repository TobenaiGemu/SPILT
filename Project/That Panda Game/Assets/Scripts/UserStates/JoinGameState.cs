using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameState : UserState
{
    private bool _joined;
    private SceneManager _sceneManager;

    public JoinGameState(User user, SceneManager sceneManager)
        :base(user)
    {
        _sceneManager = sceneManager;
    }

    public override void Initialize()
    {
        _joined = false;
    }

    public override void Update()
    {
        if (!_joined && _joystick.WasButtonPressed("Button1"))
        {
            _user.SetPlaying(true);
            _joined = true;
            GameObject.Find("Canvas").transform.Find("JoinGamePanel").transform.Find("Player" + _user.UserId + "Join").transform.Find("P" + _user.UserId).gameObject.GetComponent<Text>().text = "Player " + _user.UserId + ": Joined";
        }

        if (_joystick.WasButtonPressed("Button3"))
        {
            _sceneManager.ChangeScene<CharacterSelectScene>();
        }
    }
}
