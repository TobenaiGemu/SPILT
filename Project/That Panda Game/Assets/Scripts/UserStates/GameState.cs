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

    private Character _character;


    //Is this player currently playing

    public GameState(User user, SceneManager sceneManager)
        :base(user)
    {
        _scene = sceneManager.GetScene<GameScene>();
        
        _playerObj = GameObject.Find("Players").transform.Find("Player" + _joystick.GetId()).gameObject;
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
        _character = _user.AssignedCharacter;
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

        if (_joystick.WasButtonPressed("Button0"))
        {
            Debug.Log("Punch");
            RaycastHit hit;
            if (Physics.Raycast(_playerObj.transform.position, _playerObj.transform.forward, out hit, 5, 1<<8))
            {
                hit.transform.GetChild(0).GetComponent<Character>().ApplyKnockBack(_character.transform.forward, _character.KnockBack, _character.KnockJump);
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

        _velocity = _playerObj.transform.up * _character.ForwardSpeed * _joystick.GetAnalogue1Axis("Vertical");
        _velocity += _playerObj.transform.right * _character.ForwardSpeed * _joystick.GetAnalogue1Axis("Horizontal");


        _playerObj.transform.Rotate(new Vector3(1, 0, 0), -90);

        //Check for dead zone (so it doesnt snap back to 0 when joystick is let go)
        if (_lookDir.sqrMagnitude > 0.2f)
            _rotation = Mathf.Atan2(_lookDir.y, -_lookDir.x);

        _playerObj.transform.Rotate(0, _rotation * Mathf.Rad2Deg - 90, 0, Space.Self);

        _velocity = Vector3.Lerp(_velocity * _character.BackwardSpeed, _velocity, Mathf.InverseLerp(-1, 1, Vector3.Dot(_velocity.normalized, _playerObj.transform.forward)));

        if (_playerObj.transform.position.z > -20)
        {
            _playerObj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -10), ForceMode.Impulse);
        }

        if ((_playerObj.transform.position + _velocity * Time.deltaTime).z > -20)
        {
            return;
        }

        _playerObj.transform.position += _velocity * Time.deltaTime;
    }
}
