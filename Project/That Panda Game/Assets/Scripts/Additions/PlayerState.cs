using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : UserState
{
    GameScene _scene;

    private GameObject _playerObj;
    private GameObject _planetObj;

    //Velocity for forward/side movements
    private Vector3 _forwardVelocity;
    private Vector3 _strafeVelocity;
    private float _moveSpeed;

    private Character _character;

    //Is this player currently playing
    private bool _isPlaying;

    public PlayerState(GameScene scene, User user, bool isPlaying)
        :base(user)
    {
        _scene = scene;
        _isPlaying = isPlaying;
    }

    public override void Initialize()
    {
        _playerObj = GameObject.Find("Players").transform.Find("Player" + _joystick.GetId()).gameObject;
        _planetObj = _scene.Planet;
        _moveSpeed = 20;
    }

    // Update is called once per frame
    public override void Update ()
    {


        if (!_isPlaying)
        {
            //TODO: MAKE THIS MUCH SMALLER BY MAPPING CHARACTER TYPES TO BUTTONS
            if (_joystick.WasButtonPressed("Button1"))
            {
                if (_scene.AttemptCharacterAssign(CharacterType.Panda, _user))
                {
                    _isPlaying = true;
                    _playerObj.transform.position = new Vector3(4, 0, -30);
                    _playerObj.SetActive(true);
                }
            }

            else if (_joystick.WasButtonPressed("Button2"))
            {
                if (_scene.AttemptCharacterAssign(CharacterType.Lizard, _user))
                {
                    _isPlaying = true;
                    _playerObj.transform.position = new Vector3(2, 0, -30);
                    _playerObj.SetActive(true);
                }
            }

            else if (_joystick.WasButtonPressed("Button3"))
            {
                if (_scene.AttemptCharacterAssign(CharacterType.Elephant, _user))
                {
                    _isPlaying = true;
                    _playerObj.transform.position = new Vector3(-2, 0, -30);
                    _playerObj.SetActive(true);
                }
            }

            else if (_joystick.WasButtonPressed("Button0"))
            {
                if (_scene.AttemptCharacterAssign(CharacterType.Pig, _user))
                {
                    _isPlaying = true;
                    _playerObj.transform.position = new Vector3(-4, 0, -30);
                    _playerObj.SetActive(true);
                }
            }
            return;
        }

        //Set the velocity vectors to the correct direction and magnitude (based on input and speed)
        _forwardVelocity = _playerObj.transform.forward * _joystick.GetAnalogue1Axis("Vertical") * _moveSpeed;
        _strafeVelocity = _playerObj.transform.right * _joystick.GetAnalogue1Axis("Horizontal") * _moveSpeed;

        //add the velocities to the position
        _playerObj.transform.position += (_forwardVelocity + _strafeVelocity)  * Time.deltaTime;

        //Rotate towards the centre of the planet
        _playerObj.transform.up = _playerObj.transform.position - _planetObj.transform.position;
    }
}
