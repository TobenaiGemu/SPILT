using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Character : MonoBehaviour
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private int _coinsToWin;

    public Vector3 Position
    {
        get
        {
            return _characterObj.transform.position;
        }
        private set { }
    }

    [SerializeField]
    private float _forwardSpeed;
    public float ForwardSpeed
    {
        get
        {
            return _forwardSpeed * _forwardSpeedMultiplier;
        }
        private set { }
    }

    [SerializeField]
    private float _backwardSpeedMultiplier;
    public float BackwardSpeed
    {
        get
        {
            return _backwardSpeedMultiplier;
        }
    }

    [SerializeField]
    private float _knockBack;
    public float KnockBack
    {
        get
        {
            return _knockBack * _knockbackMultiplier;
        }
        private set { }
    }

    [SerializeField]
    private float _knockJump;
    public float KnockJump
    {
        get
        {
            return _knockJump * _knockjumpMultiplier;
        }
        private set { }
    }

    [SerializeField]
    private int _punchDropCoins;
    public int PunchDropCoins
    {
        get
        {
            return _punchDropCoins;
        }
        private set { }
    }

    [SerializeField]
    private float _punchCooldown;
    public float PunchCooldown
    {
        get
        {
            return _punchCooldown;
        }
        private set { }
    }

    [SerializeField]
    private float _leftPunchCooldown;
    public float LeftPunchCooldown
    {
        get
        {
            return _leftPunchCooldown;
        }
        private set { }
    }

    [SerializeField]
    private float _rightPunchCooldown;
    public float RightPunchCooldown
    {
        get
        {
            return _rightPunchCooldown;
        }
        private set { }
    }

    [SerializeField]
    private float _punchRadius;
    public float PunchRadius
    {
        get
        {
            return _punchRadius;
        }
        private set { }
    }

    [SerializeField]
    private float _punchDistance;
    public float PunchDistance
    {
        get
        {
            return _punchDistance;
        }
        private set { }
    }

    private Animator _animator;
    public Animator Animator
    {
        get
        {
            return _animator;
        }
        private set { }
    }

    private int _punchCoinDropModifier;
    public int PunchCoinDropModifier
    {
        get
        {
            return _punchCoinDropModifier;
        }
        private set { }
    }

    [SerializeField]
    private int _coinFrwdForceMin;
    [SerializeField]
    private int _coinFrwdForceMax;
    [SerializeField]
    private int _coinUpForceMin;
    [SerializeField]
    private int _coinUpForceMax;

    private GameObject _characterObj;
    private GameObject _characterPool;
    private User _assignedUser;
    private bool _isAssigned;

    private int _coins;


    private float _forwardSpeedMultiplier;
    private float _knockbackMultiplier;
    private float _knockjumpMultiplier;
    private float _speedMultiplierTimer;

    private float _timeToUnroast;
    private float _roastPercent;
    private TimeLerper _lerper;

    private bool _marshmallowRoasted;
    private MamaMarshmallow _mamaMarshmallow;

    private Spawner _coinSpawner;

    private GameObject _scoreText;
    private GameObject _winnerText;
    private float _winnerTime;

    //Get rid of this
    private SceneManager _sceneManager;

    public void Awake()
    {
        //Get all the objects the character depends on
        _characterPool = GameObject.Find("AvailableCharacters");
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _lerper = new TimeLerper();
        _mamaMarshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>();
        _winnerText = GameObject.Find("Canvas").transform.Find("GamePanel").Find("WINNER").gameObject;
        _winnerText.SetActive(false);
        _winnerTime = 5;
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _animator = gameObject.GetComponent<Animator>();
    }

    public Character Init(GameObject charObj)
    {
        _characterObj = charObj;
        _scoreText = GameObject.Find("Canvas").transform.Find("GamePanel").Find(_name).gameObject;
        ResetValues();
        return this;
    }

    public void ResetValues()
    {
        _forwardSpeedMultiplier = 1;
        _knockbackMultiplier = 1;
        _knockjumpMultiplier = 1;
        _speedMultiplierTimer = 1;
        _winnerTime = 5;
        _coins = 0;
    }

    public void AddCoins(int ammount)
    {
        _coins += ammount;
        UpdateCoinPanel();
        if (_coins >= _coinsToWin)
            WinGame();
    }

    public void DropCoins(int ammount)
    {
        if (ammount > _coins)
            ammount = _coins;
        _coins -= ammount;
        UpdateCoinPanel();
        //For every dropped coin, get a coin from the pool and add a force to it to give the effect of the coin being dropped
        for (int i = 0; i < ammount; i++)
        {
            GameObject coin = _coinSpawner.GetCoin();
            coin.transform.position = transform.position + transform.up * 5;
            coin.transform.LookAt(GameObject.Find("Planet").transform);
            coin.SetActive(true);
            int angle = UnityEngine.Random.Range(0, 360);
            Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
            coin.GetComponent<Rigidbody>().AddForce(direction * UnityEngine.Random.Range(_coinFrwdForceMin, _coinFrwdForceMax) + transform.up * UnityEngine.Random.Range(_coinUpForceMin, _coinUpForceMax), ForceMode.Impulse);
        }
    }

    private void UpdateCoinPanel()
    {
        _scoreText.GetComponent<Text>().text = _name + ": " + _coins;
    }

    public void ApplyKnockBack(Vector3 direction, float backForce, float upForce)
    {
        transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.parent.GetComponent<Rigidbody>().AddForce((direction.normalized * backForce + gameObject.transform.up * upForce), ForceMode.Impulse);
    }

    public void MultiplySpeed(float speedMultiplier, float duration)
    {
        //Set the speed multiplier
        _forwardSpeedMultiplier = speedMultiplier;
        _speedMultiplierTimer = duration;
        //Stop any existing timer coroutines and start a new one
        StopCoroutine(SpeedMultiplierCountdown());
        StartCoroutine(SpeedMultiplierCountdown());
    }

    public IEnumerator SpeedMultiplierCountdown()
    {
        //Set the speed multiplier back to 1 after a set time
        while (_speedMultiplierTimer > 0)
        {
            _speedMultiplierTimer -= Time.deltaTime;
            yield return null;
        }
        _forwardSpeedMultiplier = 1;
    }

    private void WinGame()
    {
        //This character won the game (ragdoll other characters, run dance animation, change scene to end game scene)
        foreach (User user in SceneManager.Users)
        {
            _winnerText.GetComponent<Text>().text = _name + " WINS!!!";
            _winnerText.SetActive(true);
        }
        StartCoroutine(FinishGame());
    }

    public IEnumerator FinishGame()
    {
        //After a set time after a player wins, return to the main menu
        while (_winnerTime > 0)
        {
            _winnerTime -= Time.deltaTime;
            yield return null;
        }
        _coins = 0;
        _sceneManager.ChangeScene<MainMenuScene>();
    }

    public bool AttemptAssignToUser(User user)
    {
        //If a user isn't already assigned to this character, assign it to this
        if (!_isAssigned)
        {
            _assignedUser = user;
            _isAssigned = true;
            _characterObj.transform.SetParent(GameObject.Find("Players").transform.Find("Player" + _assignedUser.UserId), false);
            _characterObj.transform.localPosition = Vector3.zero;
            _characterObj.SetActive(true);
            return true;
        }
        return false;
    }

    public void RoastMarshmallow(float durationToRoast, float durationToUnroast, float knockbackMultiplier, float knockjumpMultiplier, int coinDrop)
    {
        if (_marshmallowRoasted)
            return;
        _roastPercent = _lerper.Lerp(0, 100, durationToRoast);
        //TODO: Change opacity of roast texture over time
        if (_roastPercent >= 100)
        {
            _timeToUnroast = durationToUnroast;
            CompleteRoast(knockbackMultiplier, knockjumpMultiplier, coinDrop);
        }
    }

    public void StopRoastingMarshmallow()
    {
        if (_marshmallowRoasted)
            return;
        _roastPercent = 0;
        _knockbackMultiplier = 1;
        _knockjumpMultiplier = 1;
        _lerper.Reset();
        //TODO: Change texture back to normal;
    }

    private void CompleteRoast(float knockbackMultiplier, float knockjumpMultiplier, int coinDrop)
    {
        //Set the characters knockback multiplier and start a timer coroutine
        _mamaMarshmallow.GetVewyAngewy(this);
        _marshmallowRoasted = true;
        _knockbackMultiplier = knockbackMultiplier;
        _knockjumpMultiplier = knockjumpMultiplier;
        _punchCoinDropModifier = coinDrop;
        StartCoroutine(UnroastMarshmallowCounter());
    }

    public IEnumerator UnroastMarshmallowCounter()
    {
        while (_timeToUnroast > 0)
        {
            _timeToUnroast -= 1;
            yield return new WaitForSeconds(1);
        }
        _marshmallowRoasted = false;
        StopRoastingMarshmallow();
    }

    public void Unassign()
    {
        //Unassign any user that is assigned to this character
        if (_isAssigned)
        {
            _assignedUser = null;
            _isAssigned = false;
            _characterObj.transform.SetParent(_characterPool.transform, false);
            _characterObj.SetActive(false);
        }
    }
}
