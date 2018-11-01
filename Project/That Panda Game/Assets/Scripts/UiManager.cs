using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{

    private SceneManager _sceneManager;

    private Dictionary<int, GameObject> _eventSystems = new Dictionary<int, GameObject>();
    private GameObject _currentEventSystem;
    private GameObject _selectedGameObject;

    private GameScene _gameScene;

    public void Start()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        _eventSystems.Add(1, GameObject.Find("EventSystem1"));
        _eventSystems.Add(2, GameObject.Find("EventSystem2"));
        _eventSystems.Add(3, GameObject.Find("EventSystem3"));
        _eventSystems.Add(4, GameObject.Find("EventSystem4"));

        _gameScene = GameObject.Find("Scenes").transform.Find("GameScene").GetComponent<GameScene>();

        ChangeEventSystem(1);

    }

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            _selectedGameObject = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(_selectedGameObject);
    }

    public void ChangeEventSystem(int i)
    {
        foreach (GameObject es in _eventSystems.Values)
            es.SetActive(false);

        _currentEventSystem = _eventSystems[i];
        _currentEventSystem.SetActive(true);
        EventSystem.current = _eventSystems[i].GetComponent<EventSystem>();
    }

    public bool IsHovering(string btnName)
    {
        return (EventSystem.current.currentSelectedGameObject.name == btnName);
    }

    public void StartGame()
    {
        _sceneManager.ChangeScene<GameScene>();
    }

    public void GotoGamemode()
    {
        _sceneManager.ChangeScene<GamemodeScene>();
    }

    public void GotoMainMenu()
    {
        _sceneManager.ChangeScene<MainMenuScene>();
    }

    public void GotoInfo()
    {
        _sceneManager.ChangeScene<InfoScene>();
    }

    public void GotoOptions()
    {
        _sceneManager.ChangeScene<OptionsScene>();
    }

    public void ResumeGame()
    {
        _sceneManager.ResumeScene();
        _gameScene.PlayGame();
    }

    public void GotoCharacterSelect()
    {
        _sceneManager.ChangeScene<CharacterSelectScene>();
    }
}
