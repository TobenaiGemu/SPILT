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
    private float _winnerTimer;
    private float _mamaTimer;
    private bool _mamaFalling;

    private Vector3 _initCameraPos;
    private Vector3 _targetCameraPos;
    [SerializeField]
    private float _gameTime;
    private float _gameTimer;
    private Text _timerLText;
    private Text _timerRText;

    private Text _startTimerText;
    private int _startTimer;
    private TimeLerper _startLerper;

    private GameObject _winnerText;
    private bool _gameFinished;

    public void Awake()
    {
        GameObject chars = GameObject.Find("AvailableCharacters");

        Planet = GameObject.Find("Planet");
        //For lerping the planet scale
        _lerper = new TimeLerper();
        _startLerper = new TimeLerper();
        _gamePanel = GameObject.Find("Canvas").transform.Find("GamePanel").gameObject;
        _pausePanel = GameObject.Find("Canvas").transform.Find("GamePausePanel").gameObject;
        _startTimerText = _gamePanel.transform.Find("StartTimer").GetComponent<Text>();

        _timerLText = _gamePanel.transform.Find("TimerL").GetComponent<Text>();
        _timerRText = _gamePanel.transform.Find("TimerR").GetComponent<Text>();

        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _cookieSpawner = GameObject.Find("CookieSpawner").GetComponent<Spawner>();
        _appleSpawner = GameObject.Find("AppleSpawner").GetComponent<Spawner>();

        _mamaMarshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>();
        _winnerText = GameObject.Find("Canvas").transform.Find("GamePanel").Find("WINNER").gameObject;

        _activeCharacters = new List<Character>();
        _activePlayers = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            _gamePanel.transform.Find("p" + (i + 1) + "ScoreBox").gameObject.SetActive(false);
        }
    }

    public override void Initialize()
    {
        _timerLText.text = "90";
        _timerRText.text = "90";
        _winnerTimer = _winnerTime;
        _initCameraPos = Camera.main.transform.position;
        _targetCameraPos = new Vector3(0, 0, -54);
        ResetMamaTimer();
        _lerper.Reset();
        _startLerper.Reset();
        _winnerText.SetActive(false);
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
        _startTimerText.gameObject.SetActive(true);
        _startTimerText.transform.localScale = Vector3.zero;
        _gameFinished = false;
    }

    public override void Cleanup()
    {
        //Unasign the players
        foreach (User user in SceneManager.Users)
        {
            CharacterUnassign(user);
            user.SetPlaying(false);
        }
        _coinSpawner.Cleanup();
        _cookieSpawner.Cleanup();
        _appleSpawner.Cleanup();
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _lerper.Reset();
    }

    public void WinGame(Character winner)
    {
        _winnerText.GetComponent<Text>().text = "PLayer " + winner.AssignedUser.UserId + " Wins!";
        _gameFinished = true;
        _winnerText.SetActive(true);
        Debug.Log(winner.Name);
        StartCoroutine(FinishGame());
    }

    public IEnumerator FinishGame()
    {
        while (_winnerTimer > 0)
        {
            _winnerTimer -= Time.deltaTime;
            yield return null;
        }
        _mamaMarshmallow.StopMarshmallow();
        _sceneManager.ChangeScene<MainMenuScene>();
    }

    public override bool IntroTransition()
    { 
        if (Camera.main.transform.position != _targetCameraPos)
        {
            Camera.main.transform.position = _lerper.Lerp(_initCameraPos, _targetCameraPos, 0.4f);
            return false;
        }

        //Activate the game panel
        _gamePanel.gameObject.SetActive(true);

        if (_startTimer != -1)
        {
            if (_startTimerText.text != _startTimer.ToString() || _startTimerText.text != "START!")
            {
                if (_startTimer == 0)
                    _startTimerText.text = "START!";
                else
                    _startTimerText.text = _startTimer.ToString();
            }
            if (_startTimerText.transform.localScale != Vector3.one)
            {
                _startTimerText.transform.localScale = _startLerper.Lerp(Vector3.zero, Vector3.one, 1);
                return false;
            }

            _startTimerText.transform.localScale = Vector3.zero;
            _startLerper.Reset();
            _startTimer -= 1;
            return false;
        }

        //Change user states to JoinState
        foreach (User user in SceneManager.Users)
            user.ChangeState("GameState");
        _lerper.Reset();
        _startTimerText.gameObject.SetActive(false);
        return true;
    }

    public override bool OutroTransition()
    {
        ResumeGame();
        _winnerText.SetActive(false);
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
        _timerLText.text = ((int)_gameTimer).ToString();
        _timerRText.text = ((int)_gameTimer).ToString();
        _mamaTimer -= Time.deltaTime;
        if (_mamaTimer <= 0)
        {
            if (!_mamaFalling)
                _mamaMarshmallow.GetVewyAngewy();
            _mamaFalling = true;
            if (_mamaMarshmallow.HasCrashed())
            {
                ResetMamaTimer();
            }
        }

        if (_gameTimer <= 0)
        {
            Character winner = _activeCharacters[0];
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

    public void ResumeGame()
    {
        _mamaMarshmallow.Resume();
        _gamePanel.gameObject.SetActive(true);
        _pausePanel.gameObject.SetActive(false);
        Planet.GetComponent<Planet>().Resume();
    }

    public void PauseGame(User user)
    {
        Planet.GetComponent<Planet>().Pause();
        _mamaMarshmallow.Pause();
        //Stop updating and show pause menu panel
        _gamePanel.gameObject.SetActive(false);
        _pausePanel.gameObject.SetActive(true);
        _uiManager.ChangeEventSystem(user.UserId);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.firstSelectedGameObject = _pausePanel.transform.Find("Resume").gameObject;
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        _sceneManager.PauseScene();
    }
}
