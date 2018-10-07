using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : UserState
{
    GameScene _scene;

    private GameObject _playerObj;
    private GameObject _planetObj;

    //Velocity for forward/side movements
    private Vector3 _velocity;
    private Vector2 _lookDir;
    private float _rotation;
    private float _moveSpeed;

    private Character _character;

    //Is this player currently playing

    public GameState(User user, GameScene scene)
        :base(user)
    {
        _scene = scene;
        _lookDir = Vector2.zero;
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
    public override void FixedUpdate ()
    {
        if (_joystick.WasButtonPressed("Pause"))
        {
            _scene.PauseGame(_user);
            return;
        }

        //Rotate towards the centre of the planet
        _playerObj.transform.LookAt(_planetObj.transform.position);
        _playerObj.transform.Rotate(new Vector3(1, 0, 0), -90);

        //Rotate the gameobject based on input
        _lookDir.x = _joystick.GetAnalogue1Axis("Horizontal");
        _lookDir.y = _joystick.GetAnalogue1Axis("Vertical");

        //Check for dead zone (so it doesnt snap back to 0 when joystick is let go)
        if (_lookDir.sqrMagnitude > 0.2f)
            _rotation = Mathf.Atan2(_lookDir.y, -_lookDir.x);

        _playerObj.transform.Rotate(0, _rotation * Mathf.Rad2Deg - 90, 0, Space.Self);

        if (_lookDir.sqrMagnitude > 0.2f)
            _velocity = _playerObj.transform.forward * _moveSpeed;
        else
            _velocity = Vector3.zero;

        if ((_playerObj.gameObject.transform.position + _velocity * Time.deltaTime).z > -10)
            return;
        _playerObj.transform.position += _velocity * Time.deltaTime;

    }
}
