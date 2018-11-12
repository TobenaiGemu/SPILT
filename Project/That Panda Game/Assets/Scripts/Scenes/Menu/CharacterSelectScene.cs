using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectScene : Scene
{
    private GameObject _characterSelectPanel;
    private CanvasGroup _characterSelectCanvas;
    private TimeLerper _lerper;
    private GameObject _firstSelectedCharacter;

    private int _userIndex;
    private User _currentUser;

    private void Awake()
    {
        _characterSelectPanel = GameObject.Find("Canvas").transform.Find("CharacterSelectPanel").gameObject;
        _characterSelectCanvas = _characterSelectPanel.GetComponent<CanvasGroup>();
        _lerper = new TimeLerper();
        _characterSelectPanel.SetActive(false);
        _characterSelectCanvas.alpha = 0;
        _firstSelectedCharacter = GameObject.Find("Canvas").transform.Find("CharacterSelectPanel").transform.Find("Pan").gameObject;
    }

    public override void Initialize()
    {
        _lerper.Reset();
        _characterSelectPanel.SetActive(true);

        _userIndex = 1;
        for (int i = 1; i <= 4; i++)
        {
            if (_sceneManager.GetUser(i).IsPlaying)
            {
                _currentUser = _sceneManager.GetUser(i);
                _userIndex = i;
                break;
            }
        }
    }

    public override bool IntroTransition()
    {
        if (_characterSelectCanvas.alpha != 1)
        {
            _characterSelectCanvas.alpha = _lerper.Lerp(0, 1, 0.5f);
            return false;
        }
        foreach (User user in SceneManager.Users)
            user.ChangeState("CharacterSelectState");
        _lerper.Reset();
        _uiManager.ChangeEventSystem(_userIndex);
        EventSystem.current.firstSelectedGameObject = _firstSelectedCharacter;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstSelectedCharacter);
        return true;
    }

    public override bool OutroTransition()
    {
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

    public void SelectCharacter(CharacterType characterType)
    {
        if (_sceneManager.AttemptCharacterAssign(characterType, _currentUser))
            GetNextUser();
    }

    private void GetNextUser()
    {
        _userIndex++;
        if (_userIndex > 4)
        {
            _sceneManager.ChangeScene<GameScene>();
        }
        else if (_sceneManager.GetUser(_userIndex).IsPlaying)
        {
            Debug.Log(_userIndex);
            _currentUser = _sceneManager.GetUser(_userIndex);
            
            _uiManager.ChangeEventSystem(_userIndex);
            EventSystem.current.firstSelectedGameObject = _firstSelectedCharacter;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_firstSelectedCharacter);
        }
        else
        {
            GetNextUser();
        }
    }
}
