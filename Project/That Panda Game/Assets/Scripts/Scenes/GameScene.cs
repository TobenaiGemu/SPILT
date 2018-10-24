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

    private GameObject _gamePanel;
    private GameObject _pausePanel;
    private TimeLerper _lerper;

    private Spawner _coinSpawner;
    private Spawner _cookieSpawner;
    private Spawner _appleSpawner;

    public void Awake()
    {
        GameObject chars = GameObject.Find("AvailableCharacters");

        //Add characters to dictionary
        GameObject panda = chars.transform.Find("Panda").gameObject;
        _characters.Add(CharacterType.Panda, panda.GetComponent<Character>().Init(panda));

        GameObject lizard = chars.transform.Find("Lizard").gameObject;
        _characters.Add(CharacterType.Lizard, lizard.GetComponent<Character>().Init(lizard));

        GameObject elephant = chars.transform.Find("Elephant").gameObject;
        _characters.Add(CharacterType.Elephant, elephant.GetComponent<Character>().Init(elephant));

        GameObject pig = chars.transform.Find("Pig").gameObject;
        _characters.Add(CharacterType.Pig, pig.GetComponent<Character>().Init(pig));

        Planet = GameObject.Find("Planet");
        //For lerping the planet scale
        _lerper = new TimeLerper();
        _bigPlanetScale = new Vector3(50, 50, 50);
        

        _gamePanel = GameObject.Find("Canvas").transform.Find("GamePanel").gameObject;
        _pausePanel = GameObject.Find("Canvas").transform.Find("GamePausePanel").gameObject;

        _coinSpawner = GameObject.Find("CoinSpawner").GetComponent<Spawner>();
        _cookieSpawner = GameObject.Find("CookieSpawner").GetComponent<Spawner>();
        _appleSpawner = GameObject.Find("AppleSpawner").GetComponent<Spawner>();
    }

    public override void Initialize()
    {
        _lerper.Reset();
        _smallPlanetScale = Planet.transform.localScale;
    }

    public override void Cleanup()
    {
        _gamePanel.transform.Find("Panda").GetComponent<Text>().text = "Panda: Button1 to join";
        _gamePanel.transform.Find("Lizard").GetComponent<Text>().text = "Lizard: Button2 to join";
        _gamePanel.transform.Find("Elephant").GetComponent<Text>().text = "Elephant: Button3 to join";
        _gamePanel.transform.Find("Pig").GetComponent<Text>().text = "Pig: Button0 to join";
    }

    public override bool IntroTransition()
    { 
        if (Planet.transform.localScale != _bigPlanetScale)
        {
            Planet.transform.localScale = _lerper.Lerp(_smallPlanetScale, _bigPlanetScale, 1);
            return false;
        }

        //Activate the game panel
        _gamePanel.gameObject.SetActive(true);

        //Change user states to JoinState
        foreach (User user in SceneManager.Users)
            user.ChangeState("JoinState");
        _lerper.Reset();
        GameObject.Find("Events").transform.Find("MamaMarshmallow").GetComponent<MamaMarshmallow>().GetVewyAngewy();
        return true;
    }

    public override bool OutroTransition()
    {
        //Unasign the players
        foreach (User user in SceneManager.Users)
            CharacterUnassign(user);
        _coinSpawner.Cleanup();
        _cookieSpawner.Cleanup();
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _lerper.Reset();
        return true;
    }

    public override void SceneUpdate()
    {
        //Base updates the users
        _coinSpawner.Tick();
        _cookieSpawner.Tick();
        _appleSpawner.Tick();
        base.SceneUpdate();
    }

    public override void SceneFixedUpdate()
    {
        //Planet.transform.Rotate(Vector3.up, 20 * Time.deltaTime);
        base.SceneFixedUpdate();
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

    public void PlayGame()
    {
        _gamePanel.gameObject.SetActive(true);
        _pausePanel.gameObject.SetActive(false);
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
