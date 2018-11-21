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
    private float _leftPunchTimer;
    private float _rightPunchTimer;

    private bool _leftPunchHeld;
    private bool _rightPunchHeld;

    private bool _outOfBounds;

    private TimeLerper _rotationLerper;

    private Animator _animator;

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
        _animator = _playerObj.GetComponentInChildren<Animator>();
        _rotationLerper.Reset();
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
        _leftPunchTimer -= Time.deltaTime;
        _rightPunchTimer -= Time.deltaTime;

        //Check if left/right triggers are being pressed and if cooldowns are finished
        if (_punchTimer <= 0 && _leftPunchTimer <= 0 && _joystick.GetAxis("L2") >= 0.5f && !_leftPunchHeld)
        {
            _leftPunchHeld = true;
            //reset cooldowns
            _punchTimer = _character.PunchCooldown;
            _leftPunchTimer = _character.LeftPunchCooldown;

            //Trigger punch animation
            _animator.SetTrigger("punchL");
            //SphereCast in front of player
            RaycastHit hit;
            
            if (Physics.SphereCast(_playerObj.transform.position, _character.PunchRadius, _playerObj.transform.forward, out hit, _character.PunchDistance, 1 << 8))
            {
                //If SphereCast hit a player, knock that player back and make them drop coins
                hit.collider.GetComponent<Character>().ApplyKnockBack(_character.transform.forward, _character.KnockBack, _character.KnockJump);
                hit.collider.GetComponent<Character>().DropCoins(_character.PunchDropCoins + _character.RoastedPunchModifier);
                _character.transform.Find("HitEffect").GetComponent<PunchHit>().Hit();
            }
        }
        else if (_joystick.GetAxis("L2") < 0.5f)
            _leftPunchHeld = false;

        //same as above (gonna be mergerd later)
        if (_punchTimer <= 0 && _rightPunchTimer <= 0 && _joystick.GetAxis("R2") >= 0.5f && !_rightPunchHeld)
        {
            _rightPunchHeld = true;
            _punchTimer = _character.PunchCooldown; 
            _rightPunchTimer = _character.RightPunchCooldown;

            _animator.SetTrigger("punchR");
            RaycastHit hit;
            if (Physics.SphereCast(_playerObj.transform.position, _character.PunchRadius, _playerObj.transform.forward, out hit, _character.PunchDistance, 1<<8))
            {
                hit.collider.GetComponent<Character>().ApplyKnockBack(_character.transform.forward, _character.KnockBack, _character.KnockJump);
                hit.collider.GetComponent<Character>().DropCoins(_character.PunchDropCoins + _character.RoastedPunchModifier);
                _character.transform.Find("HitEffect").GetComponent<PunchHit>().Hit();
            }
        }
        else if (_joystick.GetAxis("R2") < 0.5f)
            _rightPunchHeld = false;

        //Rotate the gameobject based on input
        _lookDir.x = _joystick.GetAnalogue1Axis("Horizontal");
        _lookDir.y = _joystick.GetAnalogue1Axis("Vertical");

        float x = _joystick.GetAnalogue2Axis("Horizontal");
        float y = _joystick.GetAnalogue2Axis("Vertical");
        if (x != 0 || y != 0)
        {
            _lookDir.x = x;
            _lookDir.y = y;
        }
    }

    public override void FixedUpdate()
    {
        if (_character == null)
            return;
        //Rotate towards the centre of the planet
        _playerObj.transform.rotation = Quaternion.identity;
        _playerObj.transform.LookAt(_planetObj.transform.position);

        //Set velocity based on joystick horizontal and vertical axis'
        float vertical = _joystick.GetAnalogue1Axis("Vertical");
        float horizontal = _joystick.GetAnalogue1Axis("Horizontal");

        _velocity = _playerObj.transform.up * _character.ForwardSpeed * vertical;
        _velocity += _playerObj.transform.right * _character.ForwardSpeed * horizontal;

        _playerObj.transform.Rotate(new Vector3(1, 0, 0), -90);

        //Check for dead zone (so it doesnt snap back to 0 when joystick is let go)
        if (_lookDir.sqrMagnitude > 0.2f)
            _rotation = Mathf.Atan2(_lookDir.y, -_lookDir.x) * Mathf.Rad2Deg;

        _playerObj.transform.Rotate(0, _rotation - 90, 0, Space.Self);
        //Set movement animation based on local player movement direction
        _animator.SetFloat("velY", Vector3.Dot(_playerObj.transform.forward, _velocity.normalized));
        _animator.SetFloat("velX", Vector3.Dot(_playerObj.transform.right, _velocity.normalized));

        //Make the player linearly move slower as they move towards backwards (strafe being slower than forward, backwards being slower than strafe)
        _velocity = Vector3.Lerp(_velocity * _character.BackwardSpeed, _velocity, Mathf.InverseLerp(-1, 1, Vector3.Dot(_velocity.normalized, _playerObj.transform.forward)));

        //If the character is outside the bounds of the play area, add a force towards the play area to get them back in
        if (_playerObj.transform.position.z > _character.BorderDistance * -1)
        {
            _playerObj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -50), ForceMode.Impulse);
            _outOfBounds = true;
        }

        //if (_outOfBounds && _playerObj.transform.position.z < -15)
        //{
        //    _outOfBounds = false;
        //    _playerObj.GetComponent<CapsuleCollider>().enabled = true;
        //}

        //Stop the character from moving if it is outside the bounds of the play area
        if ((_playerObj.transform.position + _velocity * Time.deltaTime).z > _character.BorderDistance * -1)
        {
            return;
        }

        _playerObj.transform.position += _velocity * Time.deltaTime;
    }
}
