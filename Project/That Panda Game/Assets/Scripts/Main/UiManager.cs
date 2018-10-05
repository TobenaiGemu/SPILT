using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    private SceneManager _sceneManager;

    [SerializeField]
    private Canvas _mainMenu;
    [SerializeField]
    private Canvas _game;

    public void Start()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        _mainMenu.gameObject.SetActive(true);
        _game.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        _mainMenu.gameObject.SetActive(false);
        _sceneManager.ChangeScene("GameScene");
    }

    public void ExitGame()
    {
        _game.gameObject.SetActive(false);
        _sceneManager.ChangeScene("MenuScene");
    }
}
