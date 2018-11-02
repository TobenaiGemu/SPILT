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
    private int _coinDropModifier;

    private float _timeToUnroast;
    private float _roastPercent;
    private TimeLerper _lerper;

    private bool _marshmallowRoasted;
    private MamaMarshmallow _mamaMarshmallow;

    private Spawner _coinSpawner;

    private GameObject _scoreText;

    public void Awake()
    {
        _characterPool = GameObject.Find("AvailableCharacters");
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _lerper = new TimeLerper();
        Debug.Log(_name);
        _mamaMarshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>();
        _scoreText = GameObject.Find("Canvas").transform.Find("GamePanel").Find(_name).gameObject;
        AddCoins(10);
    }

    public Character Init(GameObject charObj)
    {
        _characterObj = charObj;
        _forwardSpeedMultiplier = 1;
        _knockbackMultiplier = 1;
        _knockjumpMultiplier = 1;
        _speedMultiplierTimer = 1;
        return this;
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
        ammount += _coinDropModifier;
        Debug.Log(_coinDropModifier);
        Debug.Log(ammount);
        if (ammount > _coins)
            ammount = _coins;
        _coins -= ammount;
        UpdateCoinPanel();
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
        _forwardSpeedMultiplier = speedMultiplier;
        _speedMultiplierTimer = duration;
        StopCoroutine(SpeedMultiplierCountdown());
        StartCoroutine(SpeedMultiplierCountdown());
    }

    public IEnumerator SpeedMultiplierCountdown()
    {
        while (_speedMultiplierTimer > 0)
        {
            _speedMultiplierTimer -= Time.deltaTime;
            yield return null;
        }
        _forwardSpeedMultiplier = 1;
    }

    private void WinGame()
    {
        //This character won the game (ragdoll other characters, run dance animation, change scene to end game scene
    }

    public bool AttemptAssignToUser(User user)
    {
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
        _coinDropModifier = 0;
        _lerper.Reset();
        //TODO: Change texture back to normal;
    }

    private void CompleteRoast(float knockbackMultiplier, float knockjumpMultiplier, int coinDrop)
    {
        _mamaMarshmallow.GetVewyAngewy(this);
        _marshmallowRoasted = true;
        _knockbackMultiplier = knockbackMultiplier;
        _knockjumpMultiplier = knockjumpMultiplier;
        _coinDropModifier = coinDrop;
        StartCoroutine(UnroastMarshmallowCounter());
        //TODO: Change coin drop to that and add roasted timer
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
        if (_isAssigned)
        {
            _assignedUser = null;
            _isAssigned = false;
            _characterObj.transform.SetParent(_characterPool.transform, false);
            _characterObj.SetActive(false);
        }
    }
}
