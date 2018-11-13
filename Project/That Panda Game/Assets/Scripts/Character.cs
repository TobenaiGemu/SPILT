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
    public string Name
    {
        get
        {
            return _name;
        }
        private set { }
    }

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
    private int _roastedPunchModifier;
    public int RoastedPunchModifier
    {
        get
        {
            if (!_marshmallowRoasted)
                return 0;
            return _roastedPunchModifier;
        }
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
    private float _borderDistance;
    public float BorderDistance
    {
        get
        {
            return _borderDistance;
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
    [SerializeField]
    private Sprite _scoreNameSprite;
    [SerializeField]
    private GameObject _rightMarshmallow;
    [SerializeField]
    private GameObject _leftMarshmallow;
    [SerializeField]
    private Material _burntMallowMat;
    private Material _standardMallowMat;

    private GameObject _characterObj;
    private GameObject _characterPool;
    private User _assignedUser;
    private bool _isAssigned;

    private int _coins;
    public int Coins
    {
        get
        {
            return _coins;
        }
        private set { }
    }

    private float _forwardSpeedMultiplier;
    private float _knockbackMultiplier;
    private float _knockjumpMultiplier;
    private float _speedMultiplierTimer;

    private float _unroastTimer;
    private float _timeToUnroast;
    private float _roastPercent;
    private TimeLerper _lerper;

    private bool _marshmallowRoasted;
    private MamaMarshmallow _mamaMarshmallow;

    private Spawner _coinSpawner;

    private GameObject _scoreBox;
    private float _winnerTime;
    private GameScene _gameScene;

    //Get rid of this
    private SceneManager _sceneManager;

    public void Awake()
    {
        _standardMallowMat = _leftMarshmallow.GetComponent<Renderer>().sharedMaterial;
    }

    public Character Init(GameObject charObj)
    {
        _characterPool = GameObject.Find("AvailableCharacters");
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _lerper = new TimeLerper();
        _mamaMarshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>();
        _winnerTime = 5;
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _gameScene = GameObject.Find("Scenes").transform.Find("GameScene").GetComponent<GameScene>();
        _characterObj = charObj;
        ReInit();
        return this;
    }

    public void ReInit()
    {
        _coins = 0;
        _forwardSpeedMultiplier = 1;
        _knockbackMultiplier = 1;
        _knockjumpMultiplier = 1;
        _speedMultiplierTimer = 1;
    }

    public void Cleanup()
    {
        _scoreBox.SetActive(false);
        Unassign();
    }

    public void AddCoins(int ammount)
    {
        _coins += ammount;
        UpdateCoinPanel();
        //if (_coins >= _coinsToWin)
        //    WinGame();
    }

    public void DropCoins(int ammount)
    {
        if (ammount > _coins)
            ammount = _coins;
        _coins -= ammount;
        UpdateCoinPanel();
        for (int i = 0; i < ammount; i++)
        {
            GameObject coin = _coinSpawner.SpawnObject(transform.position + transform.up * 5);
            int angle = UnityEngine.Random.Range(0, 360);
            Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
            coin.GetComponent<Rigidbody>().AddForce(direction * UnityEngine.Random.Range(_coinFrwdForceMin, _coinFrwdForceMax) + transform.up * UnityEngine.Random.Range(_coinUpForceMin, _coinUpForceMax), ForceMode.Impulse);
        }
    }

    private void UpdateCoinPanel()
    {
        _scoreBox.transform.Find("Score").GetComponent<Text>().text = "P" + _assignedUser.UserId + ": " + _coins;
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
        _gameScene.WinGame(this);
        _coins = 0;
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
            Debug.Log(_assignedUser.UserId);
            _scoreBox = GameObject.Find("Canvas").transform.Find("GamePanel").Find("p" + _assignedUser.UserId + "ScoreBox").gameObject;
            _scoreBox.transform.Find("Name").GetComponent<Image>().sprite = _scoreNameSprite;
            _scoreBox.transform.Find("Score").GetComponent<Text>().text = _name + ": 0";
            _scoreBox.SetActive(true);
            UpdateCoinPanel();
            return true;
        }
        return false;
    }

    public void RoastMarshmallow(float durationToRoast, float durationToUnroast, float knockbackMultiplier, float knockjumpMultiplier, int coinDrop)
    {
        if (_marshmallowRoasted)
            return;
        _roastPercent = _lerper.Lerp(0f, 1f, durationToRoast);
        _leftMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, _roastPercent);
        _rightMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, _roastPercent);
        //TODO: Change opacity of roast texture over time
        if (_roastPercent >= 1)
        {
            _lerper.Reset();
            _unroastTimer = durationToUnroast;
            _timeToUnroast = durationToUnroast;
            CompleteRoast(knockbackMultiplier, knockjumpMultiplier, coinDrop);
        }
    }

    public void StopRoastingMarshmallow()
    {
        if (_marshmallowRoasted)
            return;
        Debug.Log("Unroasted");

        _knockbackMultiplier = 1;
        _knockjumpMultiplier = 1;
        _lerper.Reset();
        StartCoroutine(FadeAwayRoast());
        //TODO: Change texture back to normal;
    }

    private void CompleteRoast(float knockbackMultiplier, float knockjumpMultiplier, int coinDrop)
    {
        _mamaMarshmallow.GetVewyAngewy(this);
        _marshmallowRoasted = true;
        _knockbackMultiplier = knockbackMultiplier;
        _knockjumpMultiplier = knockjumpMultiplier;
        Debug.Log("Roasted");

        StartCoroutine(UnroastMarshmallowCounter());
    }

    public IEnumerator UnroastMarshmallowCounter()
    {
        while (_unroastTimer > 0)
        {
            _unroastTimer -= 1;
            yield return new WaitForSeconds(1);
        }
        _marshmallowRoasted = false;
        StopRoastingMarshmallow();
    }

    public IEnumerator FadeAwayRoast()
    {
        float unroastPercent = _roastPercent;
        while (unroastPercent > 0)
        {
            unroastPercent = _lerper.Lerp(_roastPercent, 0, 1);
            Debug.Log(unroastPercent);
            _leftMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, unroastPercent);
            _rightMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, unroastPercent);
            yield return null;
        }
        _roastPercent = 0;
        _lerper.Reset();
    }

    private void Unassign()
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
