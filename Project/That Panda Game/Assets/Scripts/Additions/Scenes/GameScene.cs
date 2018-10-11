using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CharacterType {Panda, Lizard, Elephant, Pig}

public class GameScene : Scene
{
    private Dictionary<CharacterType, Character> _characters = new Dictionary<CharacterType, Character>();


    public GameObject Planet { get; private set; }
    private Vector3 _smallPlanetScale;
    private Vector3 _bigPlanetScale;
    private float _timeToLerpPlanet;
    private float _timeStartedLerpPlanet;
    private bool _startedLerpPlanet;

    private GameObject _gamePanel;
    private GameObject _pausePanel;


    public GameScene(SceneManager sceneManager)
        :base(sceneManager)
    {
        //Add characters to dictionary
        _characters.Add(CharacterType.Panda, new Character(CharacterType.Panda));
        _characters.Add(CharacterType.Lizard, new Character(CharacterType.Lizard));
        _characters.Add(CharacterType.Elephant, new Character(CharacterType.Elephant));
        _characters.Add(CharacterType.Pig, new Character(CharacterType.Pig));

        Planet = GameObject.Find("Planet");
        //For lerping the planet scale
        _bigPlanetScale = new Vector3(50, 50, 50);
        _smallPlanetScale = Planet.transform.localScale;
        _startedLerpPlanet = false;
        _timeToLerpPlanet = 1;

        _gamePanel = GameObject.Find("Canvas").transform.Find("GamePanel").gameObject;
        _pausePanel = GameObject.Find("Canvas").transform.Find("GamePausePanel").gameObject;

    }

    public override bool IntroTransition()
    {
        //Lerp the planet scale to the large size before the game starts.
        if (!_startedLerpPlanet)
        {
            _timeStartedLerpPlanet = Time.time;
            _startedLerpPlanet = true;
        }

        //Get the percentage that the lerp should be up to.
        float transitionTime = Time.time - _timeStartedLerpPlanet;
        float lerpPercentage = transitionTime / _timeToLerpPlanet;

        Planet.transform.localScale = Vector3.Lerp(_smallPlanetScale, _bigPlanetScale, lerpPercentage);
        if (Planet.transform.localScale != _bigPlanetScale)
        {
            return false;
        }

        //Activate the game panel
        _gamePanel.gameObject.SetActive(true);
        _startedLerpPlanet = false;

        //Change user states to JoinState
        foreach (User user in SceneManager.Users)
            user.ChangeState("JoinState");
        return true;
    }

    public override bool OutroTransition()
    {
        //Unasign the players
        foreach (User user in SceneManager.Users)
            CharacterUnassign(user);

        //Lerp the planet scale to the large size before the main menu shows
        if (!_startedLerpPlanet)
        {
            _timeStartedLerpPlanet = Time.time;
            _startedLerpPlanet = true;
        }

        //Get the percentage that the lerp should be up to.
        float transitionTime = Time.time - _timeStartedLerpPlanet;
        float lerpPercentage = transitionTime / _timeToLerpPlanet;

        Planet.transform.localScale = Vector3.Lerp(_bigPlanetScale, _smallPlanetScale, lerpPercentage);
        if (Planet.transform.localScale != _smallPlanetScale)
        {
            return false;
        }
        _startedLerpPlanet = false;
        
        return true;
    }

    public override void Initialize()
    {

    }

    public override void Update()
    {
        //Base updates the users
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public bool AttemptCharacterAssign(CharacterType type, User user)
    {
        //Check if the character is already assigned to a user
        bool isOk = user.AttemptAssignCharacter(_characters[type]);

        //Change text fields if successfully assigned
        if (isOk)
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

        return isOk;
    }

    public void CharacterUnassign(User user)
    {
        user.UnassignCharacter();
    }

    public void PauseGame(User user)
    {
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
