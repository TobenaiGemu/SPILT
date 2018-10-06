using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : UserState
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

    public GameState(User user, GameScene scene)
        :base(user)
    {
        _scene = scene;
    }

    public override void Initialize()
    {
        _playerObj = GameObject.Find("Players").transform.Find("Player" + _joystick.GetId()).gameObject;
        _planetObj = _scene.Planet;
        _moveSpeed = 20;
        _playerObj.SetActive(true);
        
    }

    public override void Cleanup()
    {
        _playerObj.SetActive(false);
        _playerObj.transform.position = GameObject.Find("PlayerSpawns").transform.Find("Player" + _joystick.GetId()).position;
        _playerObj.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    public override void Update ()
    {
        if (_joystick.WasButtonPressed("Pause"))
        {
            _scene.PauseGame(_user);
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
