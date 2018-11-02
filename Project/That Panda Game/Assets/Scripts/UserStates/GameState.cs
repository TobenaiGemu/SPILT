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

    private float _punchTimer;

    private bool _outOfBounds;

    private TimeLerper _rotationLerper;
    private float _rotationFrom;

    public GameState(User user, SceneManager sceneManager)
        :base(user)
    {
        _scene = sceneManager.GetScene<GameScene>();
        
        _playerObj = GameObject.Find("Players").transform.Find("Player" + _joystick.GetId()).gameObject;
        _planetObj = GameObject.Find("Planet");
        _rotationLerper = new TimeLerper();
    }

    public override void Initialize()
    {
        //Reset all values when initialized
        _playerObj.SetActive(true);
        _lookDir = Vector2.zero;
        _velocity = Vector3.zero;
        _playerObj.transform.position = GameObject.Find("PlayerSpawns").transform.Find("Player" + _joystick.GetId()).position;
        _playerObj.transform.rotation = Quaternion.identity;
        _rotation = 0;
        _character = _user.AssignedCharacter;
        _rotationLerper.Reset();
        if (_character != null)
            _character.ReInit();
    }

    public override void Cleanup()
    {
        _playerObj.SetActive(false);
        _playerObj.transform.position = GameObject.Find("PlayerSpawns").transform.Find("Player" + _joystick.GetId()).position;
    }

    public override void Update()
    {
        //Pause when pause button is pressed :3 :3 <3 <3
        if (_joystick.WasButtonPressed("Pause"))
        {
            _scene.PauseGame(_user);
            return;
        }

        _punchTimer -= Time.deltaTime;
        
        //Check if left/right triggers are being pressed and if cooldowns are finished
        if (_punchTimer <= 0 && _joystick.GetAxis("L2") >= 0.5f)
        {
            //reset cooldown
            _punchTimer = _character.PunchCooldown;
            Debug.Log("Left Punch");
            //SphereCast in front of player
            RaycastHit hit;
            if (Physics.SphereCast(_playerObj.transform.position, _character.PunchRadius, _playerObj.transform.forward, out hit, _character.PunchDistance, 1 << 8))
            {
                //If SphereCast hit a player, knock that player back and make them drop coins
                hit.collider.GetComponent<Character>().ApplyKnockBack(_character.transform.forward, _character.KnockBack, _character.KnockJump);
                hit.collider.GetComponent<Character>().DropCoins(_character.PunchDropCoins);
            }
        }
        //same as above (gonna be mergerd later)
        if (_punchTimer <= 0 && _joystick.GetAxis("R2") >= 0.5f)
        {
            _punchTimer = _character.PunchCooldown;
            Debug.Log("Right Punch");
            RaycastHit hit;
            if (Physics.SphereCast(_playerObj.transform.position, _character.PunchRadius, _playerObj.transform.forward, out hit, _character.PunchDistance, 1<<8))
            {
                Debug.Log(hit.collider.name);
                hit.collider.GetComponent<Character>().ApplyKnockBack(_character.transform.forward, _character.KnockBack, _character.KnockJump);
                hit.collider.GetComponent<Character>().DropCoins(_character.PunchDropCoins);
            }
        }

        //Rotate the gameobject based on input
        _lookDir.x = _joystick.GetAnalogue2Axis("Horizontal");
        _lookDir.y = _joystick.GetAnalogue2Axis("Vertical");
    }

    public override void FixedUpdate()
    {
        if (_character == null)
            return;
        //Rotate towards the centre of the planet
        _playerObj.transform.LookAt(_planetObj.transform.position);

        //Set velocity based on joystick horizontal and vertical axis'
        _velocity = _playerObj.transform.up * _character.ForwardSpeed * _joystick.GetAnalogue1Axis("Vertical");
        _velocity += _playerObj.transform.right * _character.ForwardSpeed * _joystick.GetAnalogue1Axis("Horizontal");

        _playerObj.transform.Rotate(new Vector3(1, 0, 0), -90);

        float prevRotation = _rotation;
        if (prevRotation < 0)
        {
            prevRotation += 360;
        }
        //Check for dead zone (so it doesnt snap back to 0 when joystick is let go)
        if (_lookDir.sqrMagnitude > 0.2f)
            _rotation = Mathf.Atan2(_lookDir.y, -_lookDir.x) * Mathf.Rad2Deg;

        if (_rotation < 0)
        {
            _rotation += 360;
        }

        if (_rotation != prevRotation)
        {
            _rotationLerper.Reset();
            _rotationFrom = prevRotation;
        }

        if (prevRotation > 180 && _rotation < 180)
        {
            _rotationFrom -= 360;
        }

        float rotation = _rotationLerper.Lerp(_rotationFrom, _rotation, 0.2f);

        if (rotation > 180)
            rotation -= 360;

        _playerObj.transform.Rotate(0, rotation - 90, 0, Space.Self);

        _velocity = Vector3.Lerp(_velocity * _character.BackwardSpeed, _velocity, Mathf.InverseLerp(-1, 1, Vector3.Dot(_velocity.normalized, _playerObj.transform.forward)));

        //If the character is outside the bounds of the play area, add a force towards the play area to get them back in
        //if (_playerObj.transform.position.z > -10)
        //{
        //    _playerObj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -50), ForceMode.Impulse);
        //    _playerObj.GetComponent<CapsuleCollider>().enabled = false;
        //    _outOfBounds = true;
        //}

        //if (_outOfBounds && _playerObj.transform.position.z < -15)
        //{
        //    _outOfBounds = false;
        //    _playerObj.GetComponent<CapsuleCollider>().enabled = true;
        //}

        //Stop the character from moving if it is outside the bounds of the play area
        if ((_playerObj.transform.position + _velocity * Time.deltaTime).z > -10)
        {
            return;
        }

        _playerObj.transform.position += _velocity * Time.deltaTime;
    }
}
