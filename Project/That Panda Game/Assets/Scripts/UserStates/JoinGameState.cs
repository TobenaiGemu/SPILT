using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameState : UserState
{
    private bool _joined;
    private AudioSource _selectAudio;

    public JoinGameState(User user)
        :base(user)
    {
        _selectAudio = GameObject.Find("MenuSounds").transform.Find("SelectButton").GetComponent<AudioSource>();
    }

    public override void Initialize()
    {
        _joined = false;
    }

    //Check for button presses and do the appropriate action
    public override void Update()
    {
        if (_joystick.WasButtonPressed("Button2"))
        {
            _sceneManager.ChangeScene<MainMenuScene>();
        }

        if (!_joined && _joystick.WasButtonPressed("Button1"))
        {
            _user.SetPlaying(true);
            _joined = true;
            _selectAudio.Play();
            //Set join game panel test of the player to say the player has joined
            GameObject.Find("Canvas").transform.Find("JoinGamePanel").transform.Find("Player" + _user.UserId + "Join").transform.Find("Join").gameObject.GetComponent<Text>().text = "Player " + _user.UserId + ": Joined";
        }

        if (_joystick.WasButtonPressed("Button3") && _joined)
        {
            _sceneManager.ChangeScene<CharacterSelectScene>();
        }
    }
}
