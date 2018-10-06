using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CharacterType {Panda, Lizard, Elephant, Pig}

public class GameScene : Scene
{
    private Dictionary<CharacterType, Character> _characters = new Dictionary<CharacterType, Character>();

    private SceneManager _sceneManager;

    public GameObject Planet { get; private set; }
    private Vector3 _initialPlanetScale;
    private Vector3 _targetPlanetScale;
    private float _timeToExpand;
    private bool _startedTransition;
    private float _timeStartedTransition;

    private GameObject _gamePanel;
    private GameObject _pausePanel;


    public GameScene(SceneManager sceneManager)
        :base(sceneManager)
    {
        _characters.Add(CharacterType.Panda, new Character(CharacterType.Panda));
        _characters.Add(CharacterType.Lizard, new Character(CharacterType.Lizard));
        _characters.Add(CharacterType.Elephant, new Character(CharacterType.Elephant));
        _characters.Add(CharacterType.Pig, new Character(CharacterType.Pig));
        _targetPlanetScale = new Vector3(50, 50, 50);
        _sceneManager = sceneManager;
        _gamePanel = GameObject.Find("Canvas").transform.Find("GamePanel").gameObject;
        _pausePanel = GameObject.Find("Canvas").transform.Find("GamePausePanel").gameObject;
    }

    public override bool IntroTransition()
    {
        if (!_startedTransition)
        {
            _timeStartedTransition = Time.time;
            _startedTransition = true;
        }

        float transitionTime = Time.time - _timeStartedTransition;
        float lerpPercentage = transitionTime / _timeToExpand;

        Planet.transform.localScale = Vector3.Lerp(_initialPlanetScale, _targetPlanetScale, lerpPercentage);
        if (Planet.transform.localScale != _targetPlanetScale)
        {
            return false;
        }

        _gamePanel.gameObject.SetActive(true);
        foreach (User user in SceneManager.Users)
            user.ChangeState("JoinState");
        return true;
    }

    public override void Initialize()
    {
        Planet = GameObject.Find("Planet");
        _initialPlanetScale = Planet.transform.localScale;
        _startedTransition = false;
        _timeToExpand = 1;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public bool AttemptCharacterAssign(CharacterType type, User user)
    {
        bool isOK = _characters[type].AssignToUser(user);

        if (isOK)
        {
            switch (type)
            {
                case CharacterType.Panda:
                    _gamePanel.transform.Find("Panda").GetComponent<Text>().text = "Panda: Joined";
                    break;
                case CharacterType.Lizard:
                    _gamePanel.transform.Find("Lizard").GetComponent<Text>().text = "Lizard: Joined";
                    break;
                case CharacterType.Elephant:
                    _gamePanel.transform.Find("Elephant").GetComponent<Text>().text = "Elephant: Joined";
                    break;
                case CharacterType.Pig:
                    _gamePanel.transform.Find("Pig").GetComponent<Text>().text = "Pig: Joined";
                    break;
            }
        }

        return isOK;
    }

    public void PauseGame(User user)
    {
        _gamePanel.gameObject.SetActive(false);
        _pausePanel.gameObject.SetActive(true);
        _uiManager.ChangeEventSystem(user.UserId);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.firstSelectedGameObject = _pausePanel.transform.Find("Resume").gameObject;
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        _sceneManager.PauseScene();
    }
}
