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

    //Is this player currently playing
    private bool _isPlaying;

    public PlayerState(GameScene scene, User user)
        :base(user)
    {
        _scene = scene;
    }

    public override void Initialize()
    {
        _playerObj = _scene.Player;
        _planetObj = _scene.Planet;
        _moveSpeed = 20;
    }

    // Update is called once per frame
    public override void Update ()
    {
        if (!_isPlaying)
        {
            //TODO: Check if joystick presses the join button
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
