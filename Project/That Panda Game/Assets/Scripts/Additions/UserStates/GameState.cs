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
        
        _playerObj = GameObject.Find("Players").transform.Find("Player" + _joystick.GetId()).gameObject;
        _moveSpeed = 20;
        _planetObj = _scene.Planet;
    }

    public override void Initialize()
    {
        _playerObj.SetActive(true);
        _lookDir = Vector2.zero;
        _velocity = Vector3.zero;
        _playerObj.transform.position = GameObject.Find("PlayerSpawns").transform.Find("Player" + _joystick.GetId()).position;
        _playerObj.transform.rotation = Quaternion.identity;
        _rotation = 0;
    }

    public override void Cleanup()
    {
        _playerObj.SetActive(false);
        _playerObj.transform.position = GameObject.Find("PlayerSpawns").transform.Find("Player" + _joystick.GetId()).position;
    }

    public override void Update()
    {
        if (_joystick.WasButtonPressed("Pause"))
        {
            _scene.PauseGame(_user);
            return;
        }

        Debug.DrawRay(_playerObj.transform.position, _playerObj.transform.forward);
        if (_joystick.WasButtonPressed("Button0"))
        {
            Debug.Log("Punch");
            RaycastHit hit;
            if (Physics.Raycast(_playerObj.transform.position, _playerObj.transform.forward, out hit, 10))
            {
                hit.rigidbody.AddForce((_playerObj.transform.forward + hit.rigidbody.gameObject.transform.up) * 30, ForceMode.Impulse);
                Debug.Log(hit.transform.name);
            }
        }

        //Rotate the gameobject based on input
        _lookDir.x = _joystick.GetAnalogue2Axis("Horizontal");
        _lookDir.y = _joystick.GetAnalogue2Axis("Vertical");
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        //Rotate towards the centre of the planet
        _playerObj.transform.rotation = Quaternion.identity;
        _playerObj.transform.LookAt(_planetObj.transform.position);


        _velocity = _playerObj.transform.up * _moveSpeed * _joystick.GetAnalogue1Axis("Vertical") * ((_joystick.GetAnalogue1Axis("Vertical") < 0) ? 1f : 1);
        _velocity += _playerObj.transform.right * _moveSpeed * _joystick.GetAnalogue1Axis("Horizontal") * 1f;

        _playerObj.transform.Rotate(new Vector3(1, 0, 0), -90);

        //Check for dead zone (so it doesnt snap back to 0 when joystick is let go)
        if (_lookDir.sqrMagnitude > 0.2f)
            _rotation = Mathf.Atan2(_lookDir.y, -_lookDir.x);

        _playerObj.transform.Rotate(0, _rotation * Mathf.Rad2Deg - 90, 0, Space.Self);

        _velocity = Vector3.Lerp(_velocity * 0.5f, _velocity, Mathf.InverseLerp(-1, 1, Vector3.Dot(_velocity.normalized, _playerObj.transform.forward)));

        if ((_playerObj.transform.position + _velocity * Time.deltaTime).z > -10)
            return;

        _playerObj.transform.position += _velocity * Time.deltaTime;
    }
}
