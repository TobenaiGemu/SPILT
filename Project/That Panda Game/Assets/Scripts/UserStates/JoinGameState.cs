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
        //If the user presses Button1 and hasn't already joined, set the user to playing mode
        if (!_joined && _joystick.WasButtonPressed("Button1"))
        {
            _user.SetPlaying(true);
            _joined = true;
            //Change the UI text to signify the player has joined
            GameObject.Find("Canvas").transform.Find("JoinGamePanel").transform.Find("Player" + _user.UserId + "Join").transform.Find("Join").gameObject.GetComponent<Text>().text = "Player " + _user.UserId + ": Joined";
        }

        if (_joystick.WasButtonPressed("Button3") && _joined)
        {
            _sceneManager.ChangeScene<CharacterSelectScene>();
        }
    }
}
