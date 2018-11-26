using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CharacterType {Panda, Lizard, Elephant, Pig}

public class GameScene : Scene
{
    //Get rid of this
    public GameObject Planet { get; private set; }

    private List<Character> _activeCharacters;
    private List<GameObject> _activePlayers;
    public List<GameObject> ActivePlayers
    {
        get
        {
            return _activePlayers;
        }
        private set { }
    }

    private GameObject _gamePanel;
    private GameObject _pausePanel;
    private TimeLerper _lerper;

    private Spawner _coinSpawner;
    private Spawner _cookieSpawner;
    private Spawner _appleSpawner;

    private MamaMarshmallow _mamaMarshmallow;
    [SerializeField]
    private int _minRandomMama;
    [SerializeField]
    private int _maxRandomMama;
    [SerializeField]
    private float _winnerTime;
    [SerializeField]
    private List<Sprite> _numberSprites;
    [SerializeField]
    private Sprite _startSprite;
    [SerializeField]
    private AudioSource _inGameMusic;
    [SerializeField]
    private AudioSource _mainMenuMusic;
    [SerializeField]
    private AudioSource _victoryMusic;
    [SerializeField]
    private AudioSource _startTimerMusic;
    private float _winnerTimer;
    private float _mamaTimer;
    private bool _mamaFalling;

    private Vector3 _initCameraPos;
    private Vector3 _targetCameraPos;
    [SerializeField]
    private float _gameTime;
    private float _gameTimer;

    private Image _startTimerImage;
    private int _startTimer;
    private List<Sprite> _timerSprites;
    private Image _timerLeft1;
    private Image _timerLeft2;
    private Image _timerRight1;
    private Image _timerRight2;

    private TimeLerper _startLerper;
    private TimeLerper _musicLerper;

    private Image _winnerImage;
    private bool _gameFinished;

    public void Awake()
    {
        GameObject chars = GameObject.Find("AvailableCharacters");
        _timerSprites = new List<Sprite>();
        Planet = GameObject.Find("Planet");
        //For lerping the planet scale
        _lerper = new TimeLerper();
        _startLerper = new TimeLerper();
        _musicLerper = new TimeLerper();
        _gamePanel = GameObject.Find("Canvas").transform.Find("GamePanel").gameObject;
        _pausePanel = GameObject.Find("Canvas").transform.Find("GamePausePanel").gameObject;
        _startTimerImage = _gamePanel.transform.Find("StartTimer").GetComponent<Image>();
        _timerLeft1 = _gamePanel.transform.Find("TimerLeft1").GetComponent<Image>();
        _timerLeft2 = _gamePanel.transform.Find("TimerLeft2").GetComponent<Image>();
        _timerRight1 = _gamePanel.transform.Find("TimerRight1").GetComponent<Image>();
        _timerRight2 = _gamePanel.transform.Find("TimerRight2").GetComponent<Image>();
        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _cookieSpawner = GameObject.Find("CookieSpawner").GetComponent<Spawner>();
        _appleSpawner = GameObject.Find("AppleSpawner").GetComponent<Spawner>();

        _mamaMarshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>();
        _winnerImage = _gamePanel.transform.Find("Player").GetComponent<Image>();

        _activeCharacters = new List<Character>();
        _activePlayers = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            _gamePanel.transform.Find("p" + (i + 1) + "ScoreBox").gameObject.SetActive(false);
        }
    }

    private void IntToSprite(int num, List<Sprite> _sprites)
    {
        //If it's a single digit then just grab the sprite right away
        _sprites.Clear();
        if (num < 10)
        {
            _sprites.Add(_numberSprites[0]);
            _sprites.Add(_numberSprites[num]);
            return;
        }
        //Gets the two number sprites that correspond to the two digit number from num
        int tenner = (int)Mathf.Floor(num / 10);
        int single = num % 10;
        _sprites.Add(_numberSprites[tenner]);
        _sprites.Add(_numberSprites[single]);
    }

    public override void Initialize()
    {
        //Reset all values
        _winnerTimer = _winnerTime;
        _initCameraPos = Camera.main.transform.position;
        _targetCameraPos = new Vector3(0, 0, -54);
        ResetMamaTimer();
        _lerper.Reset();
        _startLerper.Reset();
        _musicLerper.Reset();
        _winnerImage.gameObject.SetActive(false);
        _activeCharacters.Clear();
        foreach (User user in SceneManager.Users)
        {
            if (user.IsPlaying)
            {
                _activeCharacters.Add(user.AssignedCharacter);
                _activePlayers.Add(user.AssignedCharacter.transform.parent.gameObject);
                user.AssignedCharacter.ReInit();
            }
        }

        _gameTimer = _gameTime;
        _startTimer = 3;
        _startTimerImage.gameObject.SetActive(true);
        _startTimerImage.transform.localScale = Vector3.zero;
        _gameFinished = false;
        _timerLeft1.gameObject.SetActive(false);
        _timerLeft2.gameObject.SetActive(false);
        _timerRight1.gameObject.SetActive(false);
        _timerRight2.gameObject.SetActive(false);
    }

    public override void Cleanup()
    {
        //Unasign the players
        foreach (User user in SceneManager.Users)
        {
            CharacterUnassign(user);
            user.SetPlaying(false);
        }
        //Deactivate game panels
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _lerper.Reset();
    }

    public void WinGame(Character winner)
    {
        //Remove all items from world
        _coinSpawner.Cleanup();
        _cookieSpawner.Cleanup();
        _appleSpawner.Cleanup();
        _gameFinished = true;
        //Set winner sprite to correct player number
        _winnerImage.gameObject.SetActive(true);
        _winnerImage.transform.Find("Number").GetComponent<Image>().sprite = _numberSprites[winner.AssignedUser.UserId];
        winner.WinGame();
        //Set every other character to lose
        foreach (User user in SceneManager.Users)
        {
            if (user.IsPlaying)
            {
                //LoseGame function runs the losing animation
                if (user.AssignedCharacter != winner)
                    user.AssignedCharacter.LoseGame();
            }
        }
        _victoryMusic.Play();
        _mamaMarshmallow.StopMarshmallow();
        //Disable the game timers
        _timerLeft1.gameObject.SetActive(false);
        _timerLeft2.gameObject.SetActive(false);
        _timerRight1.gameObject.SetActive(false);
        _timerRight2.gameObject.SetActive(false);
        StartCoroutine(FinishGame());
    }

    //Go back to main menu a certain time after the winner is decided
    public IEnumerator FinishGame()
    {
        while (_winnerTimer > 0)
        {
            _winnerTimer -= Time.deltaTime;
            yield return null;
        }
        _sceneManager.ChangeScene<MainMenuScene>();
    }

    public override bool IntroTransition()
    { 
        //Move camera closer to planet
        if (Camera.main.transform.position != _targetCameraPos)
        {
            Camera.main.transform.position = _lerper.Lerp(_initCameraPos, _targetCameraPos, 0.4f);
            return false;
        }

        //Activate the game panel
        _gamePanel.gameObject.SetActive(true);

        //3...2...1...START before the users change to the game state
        if (_startTimer != -1)
        {
            //Decrease main menu volume to 0 over time
            _mainMenuMusic.volume = _musicLerper.Lerp(1, 0, 4);
            if (_startTimerImage.sprite != _numberSprites[_startTimer] || _startTimerImage.sprite != _startSprite)
            {
                if (!_startTimerMusic.isPlaying)
                    _startTimerMusic.Play();
                
                //If the timer is 0, show the START sprite
                if (_startTimer == 0)
                {
                    _startTimerImage.sprite = _startSprite;
                    _startTimerImage.SetNativeSize();
                }
                //Get the sprite that corresponds to the start timers current time
                else
                {
                    _startTimerImage.sprite = _numberSprites[_startTimer];
                    _startTimerImage.SetNativeSize();
                }
            }
            if (_startTimerImage.transform.localScale != Vector3.one)
            {
                _startTimerImage.transform.localScale = _startLerper.Lerp(Vector3.zero, Vector3.one, 1);
                return false;
            }
            _startTimerImage.transform.localScale = Vector3.zero;
            _startLerper.Reset();
            _startTimer -= 1;
            return false;
        }

        //Change user states to GameState
        foreach (User user in SceneManager.Users)
            user.ChangeState<GameState>();
        _lerper.Reset();
        _mainMenuMusic.Pause();
        _inGameMusic.Play();
        _startTimerImage.gameObject.SetActive(false);

        //Activate game timers
        _timerLeft1.gameObject.SetActive(true);
        _timerLeft2.gameObject.SetActive(true);
        _timerRight1.gameObject.SetActive(true);
        _timerRight2.gameObject.SetActive(true);

        IntToSprite(90, _timerSprites);
        _timerLeft1.sprite = _timerSprites[0];
        _timerLeft2.sprite = _timerSprites[1];
        _timerLeft1.SetNativeSize();
        _timerLeft2.SetNativeSize();

        _timerRight1.sprite = _timerLeft1.sprite;
        _timerRight2.sprite = _timerLeft2.sprite;
        _timerRight1.SetNativeSize();
        _timerRight2.SetNativeSize();
        _startTimerMusic.Stop();
        return true;
    }

    public override bool OutroTransition()
    {
        //Cleanup items and stop music
        ResumeGame();
        _winnerImage.gameObject.SetActive(false);
        _inGameMusic.Stop();
        _coinSpawner.Cleanup();
        _cookieSpawner.Cleanup();
        _appleSpawner.Cleanup();
        _gameFinished = true;
        _mamaMarshmallow.StopMarshmallow();
        return true;
    }

    public override void SceneUpdate()
    {
        if (_gameFinished)
            return;
        //Base updates the users
        _coinSpawner.Tick();
        _cookieSpawner.Tick();
        _appleSpawner.Tick();
        _gameTimer -= Time.deltaTime;
        _mamaTimer -= Time.deltaTime;

        //Change game timer sprites to the numbers that correspond to the game timer int
        IntToSprite((int)_gameTimer, _timerSprites);
        _timerLeft1.sprite = _timerSprites[0];
        _timerLeft2.sprite = _timerSprites[1];
        _timerLeft1.SetNativeSize();
        _timerLeft2.SetNativeSize();

        _timerRight1.sprite = _timerLeft1.sprite;
        _timerRight2.sprite = _timerLeft2.sprite;
        _timerRight1.SetNativeSize();
        _timerRight2.SetNativeSize();

        //Start the mama marshmallow crash
        if (_mamaTimer <= 0)
        {
            if (!_mamaFalling)
                _mamaMarshmallow.GetVewyAngewy();
            _mamaFalling = true;
        }

        //Get the winner after the game timer is up
        if (_gameTimer <= 0)
        {
            Character winner = _activeCharacters[0];
            Debug.Log(_activeCharacters.Count);
            foreach (Character chr in _activeCharacters)
            {
                if (chr.Coins > winner.Coins)
                    winner = chr;
            }
            WinGame(winner);
        }

        base.SceneUpdate();
    }

    public override void SceneFixedUpdate()
    {
        if (_gameFinished)
            return;
        base.SceneFixedUpdate();
    }

    public void ResetMamaTimer()
    {
        _mamaFalling = false;
        _mamaTimer = Random.Range(_minRandomMama, _maxRandomMama);
    }

    public void CharacterUnassign(User user)
    {
        user.UnassignCharacter();
    }

    //Resume the game after it has been paused
    public void ResumeGame()
    {
        Time.timeScale = 1;
        _inGameMusic.Play();
        _mamaMarshmallow.Resume();
        _gamePanel.gameObject.SetActive(true);
        _pausePanel.gameObject.SetActive(false);
        Planet.GetComponent<Planet>().Resume();
    }

    //Pause the game
    public void PauseGame(User user)
    {
        Time.timeScale = 0;
        _inGameMusic.Pause();
        Planet.GetComponent<Planet>().Pause();
        _mamaMarshmallow.Pause();
        //Stop updating and show pause menu panel
        _gamePanel.gameObject.SetActive(false);
        _pausePanel.gameObject.SetActive(true);
        _uiManager.ChangeEventSystem(user.UserId);
        _uiManager.DontPlayFirstHighlightSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.firstSelectedGameObject = _pausePanel.transform.Find("Resume").gameObject;
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        _sceneManager.PauseScene();
    }
}
