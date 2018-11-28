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
    private ParticleSystem _speedParticles;
    [SerializeField]
    private ParticleSystem _fireParticlesLeft;
    [SerializeField]
    private ParticleSystem _fireParticlesRight;

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

    private GameObject _crown;
    private GameObject _characterObj;
    private GameObject _characterPool;
    private User _assignedUser;
    public User AssignedUser
    {
        get
        {
            return _assignedUser;
        }
        private set { }
    }
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

    private float _unroastTimer;
    private float _timeToUnroast;
    private float _roastPercent;
    private TimeLerper _roastLerper;

    private bool _multiplySpeed;

    private float _speedParticleTimer;
    private float _speedMultiplierTimer;

    private bool _marshmallowRoasted;
    private MamaMarshmallow _mamaMarshmallow;

    private Spawner _coinSpawner;

    private GameObject _scoreBox;
    private float _winnerTime;
    private GameScene _gameScene;

    private ParticleSystem _sickBubblesPs;

    private Animator _animator;

    private AudioSource _roastedMallowSound;

    private SceneManager _sceneManager;

    public void Awake()
    {

    }

    //Called once at the start of the game
    public Character Init(GameObject charObj)
    {
        //Find all objects needed
        _standardMallowMat = _leftMarshmallow.GetComponent<Renderer>().sharedMaterial;
        _roastedMallowSound = GameObject.Find("InGameSounds").transform.Find("RoastedMarshmallows").GetComponent<AudioSource>();

        _characterPool = GameObject.Find("AvailableCharacters");

        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _mamaMarshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>();
        _winnerTime = 5;
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _gameScene = GameObject.Find("Scenes").transform.Find("GameScene").GetComponent<GameScene>();
        _sickBubblesPs = transform.Find("AppleBubblesPS").GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();

        _crown = transform.Find("Crown").gameObject;
        _crown.SetActive(false);
        _roastLerper = new TimeLerper();
        _characterObj = charObj;
        return this;
    }

    //Called before each match
    public void ReInit()
    {
        //Reset all values
        _crown.SetActive(false);
        _coins = 0;
        _forwardSpeedMultiplier = 1;
        _knockbackMultiplier = 1;
        _knockjumpMultiplier = 1;
        _speedMultiplierTimer = 1;
        UpdateCoinPanel();
        _roastLerper.Reset();
    }

    public void Cleanup()
    {
        _scoreBox.SetActive(false);
        Unassign();
    }

    public void Update()
    {
        //Do all timer stuff
        _speedParticleTimer -= Time.deltaTime;
        _speedMultiplierTimer -= Time.deltaTime;

        if (_speedParticles.isPlaying && _speedParticleTimer <= 0)
            _speedParticles.Stop();

        if (_multiplySpeed && _speedMultiplierTimer <= 0)
        {
            _animator.SetFloat("SpeedMultiplier", 1);
            _forwardSpeedMultiplier = 1;
            _multiplySpeed = false;
        }
    }

    public void AddCoins(int ammount)
    {
        _coins += ammount;

        Character first = this;
        foreach (User user in SceneManager.Users)
        {
            if (user.IsPlaying)
            {
                _crown.SetActive(false);
                if (user.AssignedCharacter.Coins > first.Coins)
                    first = user.AssignedCharacter;
                else
                    user.AssignedCharacter.DisableCrown();
            }
        }

        first.EnableCrown();

        UpdateCoinPanel();
    }

    public void EnableCrown()
    {
        _crown.SetActive(true);
    }

    public void DisableCrown()
    {
        _crown.SetActive(false);
    }

    //Update the character score and spawn/chuck gummy bears away from the character in a random direction
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

    //Adds a force to the character in a direction
    public void ApplyKnockBack(Vector3 direction, float backForce, float upForce)
    {
        transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.parent.GetComponent<Rigidbody>().AddForce((direction.normalized * backForce + gameObject.transform.up * upForce), ForceMode.Impulse);
    }

    public void StartSpeedParticles(float duration)
    {
        _speedParticles.Play();
        _speedParticleTimer = duration;
    }

    public void StartSickBubbles()
    {
        _sickBubblesPs.Play();
    }

    public void MultiplySpeed(float speedMultiplier, float duration)
    {
        _multiplySpeed = true;
        _forwardSpeedMultiplier = speedMultiplier;
        _speedMultiplierTimer = duration;
        _animator.SetFloat("SpeedMultiplier", speedMultiplier);
    }

    public void WinGame()
    {
        _animator.SetTrigger("Victory");
    }

    public void LoseGame()
    {
        _animator.SetTrigger("Lose");
    }

    //If this character is not already attached to a user, attach it to the user and initialize this character to be ready to be playable
    public bool AttemptAssignToUser(User user)
    {
        if (!_isAssigned)
        {
            _assignedUser = user;
            _isAssigned = true;
            //Parent this character to the 'Player' object and reset its position
            _characterObj.transform.SetParent(GameObject.Find("Players").transform.Find("Player" + _assignedUser.UserId), false);
            _characterObj.transform.localPosition = Vector3.zero;
            _characterObj.SetActive(true);
            //Setup the score box for this character
            _scoreBox = GameObject.Find("Canvas").transform.Find("GamePanel").Find("p" + _assignedUser.UserId + "ScoreBox").gameObject;
            _scoreBox.transform.Find("Name").GetComponent<Image>().sprite = _scoreNameSprite;
            _scoreBox.transform.Find("Score").GetComponent<Text>().text = _name + ": 0";
            _scoreBox.SetActive(true);
            UpdateCoinPanel();
            return true;
        }
        return false;
    }

    //Called every frame that the marshmallow is roasting to roast over time
    public void RoastMarshmallow(float durationToRoast, float durationToUnroast, float knockbackMultiplier, float knockjumpMultiplier, int coinDrop)
    {
        if (_marshmallowRoasted)
            return;
        //Lerp the material from the non roasted material to the roasted material
        _roastPercent = _roastLerper.Lerp(0f, 1f, durationToRoast);
        _leftMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, _roastPercent);
        _rightMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, _roastPercent);

        //If the marshmallow is fully roaster, reset the timers and complete the roast
        if (_roastPercent >= 1)
        {
            _roastLerper.Reset();
            _unroastTimer = durationToUnroast;
            _timeToUnroast = durationToUnroast;
            CompleteRoast(knockbackMultiplier, knockjumpMultiplier, coinDrop);
        }
    }

    //Called if the marshmallows stop roasting before they are fully roasted
    public void StopRoastingMarshmallow()
    {
        if (_marshmallowRoasted)
            return;

        //Stop fire particles and reset multipliers
        _fireParticlesLeft.Stop();
        _fireParticlesRight.Stop();
        _knockbackMultiplier = 1;
        _knockjumpMultiplier = 1;
        _roastLerper.Reset();
        StartCoroutine(FadeAwayRoast());
        //TODO: Change texture back to normal;
    }

    //Start fire particles and increase knockback/knockjump multipliers and coin drop
    private void CompleteRoast(float knockbackMultiplier, float knockjumpMultiplier, int coinDrop)
    {
        _fireParticlesLeft.Play();
        _fireParticlesRight.Play();
        _roastedMallowSound.Play();
        _mamaMarshmallow.GetVewyAngewy(this);
        _marshmallowRoasted = true;
        _knockbackMultiplier = knockbackMultiplier;
        _knockjumpMultiplier = knockjumpMultiplier;
        Debug.Log("Roasted");

        StartCoroutine(UnroastMarshmallowCounter());
    }

    //Unroast roasted marshmallows over time
    public IEnumerator UnroastMarshmallowCounter()
    {
        if (_sceneManager.IsPaused())
            yield return null;
        while (_unroastTimer > 0)
        {
            _unroastTimer -= 1;
            yield return new WaitForSeconds(1);
        }
        _marshmallowRoasted = false;
        StopRoastingMarshmallow();
    }

    //While the marshmallows are unroasting, fade from the roasted material to the unroasted material
    public IEnumerator FadeAwayRoast()
    {
        if (_sceneManager.IsPaused())
            yield return null;
        float unroastPercent = _roastPercent;
        while (unroastPercent > 0)
        {
            unroastPercent = _roastLerper.Lerp(_roastPercent, 0, 1);
            _leftMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, unroastPercent);
            _rightMarshmallow.GetComponent<Renderer>().material.Lerp(_standardMallowMat, _burntMallowMat, unroastPercent);
            yield return null;
        }
        _roastPercent = 0;
        _roastLerper.Reset();
    }

    //If this character is assigned to a user, unassign it
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
