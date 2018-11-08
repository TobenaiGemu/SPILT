using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CharacterType {Panda, Lizard, Elephant, Pig}

public class GameScene : Scene
{

    public GameObject Planet { get; private set; }

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
    private float _mamaTimer;
    private bool _mamaFalling;

    private Vector3 _initCameraPos;
    private Vector3 _targetCameraPos;
    [SerializeField]
    private float _gameTime;

    private GameObject _winnerText;

    public void Awake()
    {
        GameObject chars = GameObject.Find("AvailableCharacters");

        Planet = GameObject.Find("Planet");
        //For lerping the planet scale
        _lerper = new TimeLerper();        

        _gamePanel = GameObject.Find("Canvas").transform.Find("GamePanel").gameObject;
        _pausePanel = GameObject.Find("Canvas").transform.Find("GamePausePanel").gameObject;

        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _cookieSpawner = GameObject.Find("CookieSpawner").GetComponent<Spawner>();
        _appleSpawner = GameObject.Find("AppleSpawner").GetComponent<Spawner>();

        _mamaMarshmallow = GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>();
        _winnerText = GameObject.Find("Canvas").transform.Find("GamePanel").Find("WINNER").gameObject;
    }

    public override void Initialize()
    {
        _initCameraPos = Camera.main.transform.position;
        _targetCameraPos = new Vector3(0, 0, -54);
        ResetMamaTimer();
        _lerper.Reset();
        _winnerText.SetActive(false);
        foreach (User user in SceneManager.Users)
        {
            if (user.IsPlaying)
                user.AssignedCharacter.ReInit();
        }
    }

    public override void Cleanup()
    {
        //Unasign the players
        foreach (User user in SceneManager.Users)
            CharacterUnassign(user);
        _coinSpawner.Cleanup();
        _cookieSpawner.Cleanup();
        _appleSpawner.Cleanup();
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _lerper.Reset();
    }

    public void WinGame()
    {
        StartCoroutine(FinishGame());
    }

    public IEnumerator FinishGame()
    {
        while (_winnerTime > 0)
        {
            _winnerTime -= Time.deltaTime;
            Debug.Log(_winnerTime);
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

        //Change user states to JoinState
        foreach (User user in SceneManager.Users)
            user.ChangeState("GameState");
        _lerper.Reset();
        return true;
    }

    public override bool OutroTransition()
    {

        return true;
    }

    public override void SceneUpdate()
    {
        //Base updates the users
        _coinSpawner.Tick();
        _cookieSpawner.Tick();
        _appleSpawner.Tick();
        _gameTime -= Time.deltaTime;
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

        if (_gameTime <= 0)
        {
            _sceneManager.ChangeScene<MainMenuScene>();
        }

        base.SceneUpdate();
    }

    public override void SceneFixedUpdate()
    {
        //Planet.transform.Rotate(Vector3.up, 20 * Time.deltaTime);
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
