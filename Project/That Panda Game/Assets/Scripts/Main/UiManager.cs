using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{

    private SceneManager _sceneManager;

    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _game;
    [SerializeField]
    private GameObject _gamePause;

    private Dictionary<int, GameObject> _eventSystems = new Dictionary<int, GameObject>();
    private GameObject _currentEventSystem;

    public void Start()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        _eventSystems.Add(1, GameObject.Find("EventSystem1"));
        _eventSystems.Add(2, GameObject.Find("EventSystem2"));
        _eventSystems.Add(3, GameObject.Find("EventSystem3"));
        _eventSystems.Add(4, GameObject.Find("EventSystem4"));

        ChangeEventSystem(1);

        _mainMenu.SetActive(true);
        _game.SetActive(false);
    }

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }

    public void ChangeEventSystem(int i)
    {
        foreach (GameObject es in _eventSystems.Values)
            es.SetActive(false);

        _currentEventSystem = _eventSystems[i];
        _currentEventSystem.SetActive(true);
        EventSystem.current = _eventSystems[i].GetComponent<EventSystem>();
    }

    public void StartGame()
    {
        _mainMenu.SetActive(false);
        _sceneManager.ChangeScene("GameScene");
    }

    public void QuitGame()
    {
        _game.SetActive(false);
        _gamePause.SetActive(false);
        _sceneManager.ChangeScene("MenuScene");
    }

    public void ResumeGame()
    {
        _gamePause.SetActive(false);
        _game.SetActive(true);
        _sceneManager.ResumeScene();
    }
}
