﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectScene : Scene
{
    private GameObject _characterSelectPanel;
    private CanvasGroup _characterSelectCanvas;
    private TimeLerper _lerper;

    public List<GameObject> _buttons;
    private GameObject _firstSelectedCharacter;

    private int _userIndex;
    private User _currentUser;

    private bool _stopCoroutines;

    private void Awake()
    {
        _characterSelectPanel = GameObject.Find("Canvas").transform.Find("CharacterSelectPanel").gameObject;
        _characterSelectCanvas = _characterSelectPanel.GetComponent<CanvasGroup>();
        _lerper = new TimeLerper();
        _characterSelectPanel.SetActive(false);
        _characterSelectCanvas.alpha = 0;
        _buttons = new List<GameObject>();
        _buttons.Add(_characterSelectPanel.transform.Find("Pan").gameObject);
        _buttons.Add(_characterSelectPanel.transform.Find("Ham").gameObject);
        _buttons.Add(_characterSelectPanel.transform.Find("Eli").gameObject);
        _buttons.Add(_characterSelectPanel.transform.Find("Liz").gameObject);
        _firstSelectedCharacter = _buttons[0];
    }

    public override void Initialize()
    {
        //Initialize buttons
        _stopCoroutines = true;
        _lerper.Reset();
        _characterSelectPanel.SetActive(true);

        _characterSelectPanel.transform.Find("HamGrey").gameObject.SetActive(false);
        _characterSelectPanel.transform.Find("PanGrey").gameObject.SetActive(false);
        _characterSelectPanel.transform.Find("EliGrey").gameObject.SetActive(false);
        _characterSelectPanel.transform.Find("LizGrey").gameObject.SetActive(false);

        _characterSelectPanel.transform.Find("Pan").gameObject.SetActive(true);
        _characterSelectPanel.transform.Find("Ham").gameObject.SetActive(true);
        _characterSelectPanel.transform.Find("Eli").gameObject.SetActive(true);
        _characterSelectPanel.transform.Find("Liz").gameObject.SetActive(true);

        _userIndex = 1;
        for (int i = 1; i <= 4; i++)
        {
            if (_sceneManager.GetUser(i).IsPlaying)
            {
                _currentUser = _sceneManager.GetUser(i);
                _userIndex = i;
                SetPlayerText("Player " + i);
                break;
            }
        }
    }

    public override void Cleanup()
    {
        if (_sceneManager.CheckNextScene<JoinGameScene>())
        {
            foreach (User user in SceneManager.Users)
                user.UnassignCharacter();
        }
    }

    private void SetPlayerText(string text)
    {
        //Set all the current player number text in the character select canvas
        _characterSelectPanel.transform.Find("Pan").transform.Find("PlayerText").GetComponent<Text>().text = text;
        _characterSelectPanel.transform.Find("Ham").transform.Find("PlayerText").GetComponent<Text>().text = text;
        _characterSelectPanel.transform.Find("Eli").transform.Find("PlayerText").GetComponent<Text>().text = text;
        _characterSelectPanel.transform.Find("Liz").transform.Find("PlayerText").GetComponent<Text>().text = text;
    }

    public override bool IntroTransition()
    {
        //Fade in UI
        if (_characterSelectCanvas.alpha != 1)
        {
            _characterSelectCanvas.alpha = _lerper.Lerp(0, 1, 0.5f);
            return false;
        }
        foreach (User user in SceneManager.Users)
            user.ChangeState<CharacterSelectState>();
        _lerper.Reset();
        _uiManager.ChangeEventSystem(_userIndex);
        _uiManager.DontPlayFirstHighlightSound();
        EventSystem.current.firstSelectedGameObject = _firstSelectedCharacter;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedCharacter);
        return true;
    }

    public override bool OutroTransition()
    {
        if (_stopCoroutines)
        {
            StopAllCoroutines();
            _stopCoroutines = false;
        }
        //Fade out UI
        if (_characterSelectCanvas.alpha != 0)
        {
            _characterSelectCanvas.alpha = _lerper.Lerp(1, 0, 0.5f);
            return false;
        }
        _lerper.Reset();
        foreach (string character in SceneManager.CharacterNames)
        {
            _characterSelectPanel.transform.Find(character).GetComponent<CanvasGroup>().alpha = 0;
            _characterSelectPanel.transform.Find(character).GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
        _characterSelectPanel.SetActive(false);
        return true;
    }

    public bool SelectCharacter(CharacterType characterType)
    {
        if (_sceneManager.AttemptCharacterAssign(characterType, _currentUser))
        {
            //Find character that was selected and change its button to grey
            switch (characterType)
            {
                case CharacterType.Elephant:
                    _characterSelectPanel.transform.Find("EliGrey").gameObject.SetActive(true);
                    _characterSelectPanel.transform.Find("Eli").gameObject.SetActive(false);
                    break;
                case CharacterType.Pig:
                    _characterSelectPanel.transform.Find("HamGrey").gameObject.SetActive(true);
                    _characterSelectPanel.transform.Find("Ham").gameObject.SetActive(false);
                    break;
                case CharacterType.Lizard:
                    _characterSelectPanel.transform.Find("LizGrey").gameObject.SetActive(true);
                    _characterSelectPanel.transform.Find("Liz").gameObject.SetActive(false);
                    break;
                case CharacterType.Panda:
                    _characterSelectPanel.transform.Find("PanGrey").gameObject.SetActive(true);
                    _characterSelectPanel.transform.Find("Pan").gameObject.SetActive(false);
                    break;
            }
            GetNextUser();
            return true;
        }
        return false;
    }

    public override void SceneUpdate()
    {
        base.SceneUpdate();
    }

    private IEnumerator StartGame()
    {
        float startTimer = 2;
        while (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
            yield return null;
        }
        _sceneManager.ChangeScene<GameScene>();
    }

    private void GetNextUser()
    {
        //Get the next user that has joined the game and give them control of the UI
        _userIndex++;
        if (_userIndex > 4)
        {
            StartCoroutine(StartGame());
        }
        else if (_sceneManager.GetUser(_userIndex).IsPlaying)
        {
            Debug.Log(_userIndex);
            _currentUser = _sceneManager.GetUser(_userIndex);
            //Set text on top of chocolate to the current player choosing
            SetPlayerText("Player " + _userIndex);
            _uiManager.ChangeEventSystem(_userIndex);
            foreach (GameObject button in _buttons)
            {
                if (button.activeSelf)
                {
                    _uiManager.DontPlayFirstHighlightSound();
                    EventSystem.current.firstSelectedGameObject = button;
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(button);
                    break;
                }
            }
        }
        else
        {
            GetNextUser();
        }
    }
}
