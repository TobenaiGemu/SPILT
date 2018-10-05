using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    private SceneManager _sceneManager;

    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _game;
    [SerializeField]
    private GameObject _gamePause;

    public void Start()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        _mainMenu.SetActive(true);
        _game.SetActive(false);
    }

    public void StartGame()
    {
        _mainMenu.SetActive(false);
        _sceneManager.ChangeScene("GameScene");
    }

    public void QuitGame()
    {
        _game.SetActive(false);
        _sceneManager.ChangeScene("MenuScene");
    }

    public void ResumeGame()
    {
        _gamePause.SetActive(false);
        _game.SetActive(true);
        _sceneManager.ResumeScene();
    }
}
