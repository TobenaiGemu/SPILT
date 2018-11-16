using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuScene : Scene
{
    private GameObject _mainMenuPanel;
    private CanvasGroup _mainMenuCanvas;
    private GameObject _startButton;

    private CanvasGroup _leftMenuBox;
    private CanvasGroup _rightMenuBox;

    private GameObject _planet;

    private Vector3 _initCameraPos;
    private Vector3 _targetCameraPos;

    private bool _finishedScale;
    private TimeLerper _lerper;

    private MenuPage _menuPage;

    public void Awake()
    {
        Camera.main.transform.position = new Vector3(0, 0, -1100);
        _targetCameraPos = new Vector3(0, 0, -100);

        _lerper = new TimeLerper();
        _planet = GameObject.Find("Planet");

        _mainMenuPanel = GameObject.Find("Canvas").transform.Find("MainMenuPanel").gameObject;
        _leftMenuBox = GameObject.Find("Canvas").transform.Find("Left Menu Box").GetComponent<CanvasGroup>();
        _rightMenuBox = GameObject.Find("Canvas").transform.Find("Right Menu Box").GetComponent<CanvasGroup>();
        _mainMenuCanvas = _mainMenuPanel.GetComponent<CanvasGroup>();
        _startButton = _mainMenuPanel.transform.Find("Start").gameObject;
        _leftMenuBox.alpha = 0;
        _rightMenuBox.alpha = 0;
        _mainMenuCanvas.alpha = 0;
        _menuPage = new MenuPage(_mainMenuPanel);
        _menuPage.SetRightPanel("PlayRightPanel", 0);
        _menuPage.SetRightPanel("InfoRightPanel", 1);
        _menuPage.SetRightPanel("OptionsRightPanel", 2);
        _menuPage.SetRightPanel("QuitRightPanel", 3);
    }

    public override void Initialize()
    {
        _initCameraPos = Camera.main.transform.position;
        _rightMenuBox.gameObject.SetActive(true);
        _leftMenuBox.gameObject.SetActive(true);
        _mainMenuCanvas.alpha = 0;
        _menuPage.Initialize();
        _lerper.Reset();
    }

    public override bool IntroTransition()
    {
        _menuPage.Update();
        if (Camera.main.transform.position != _targetCameraPos)
        {
            Camera.main.transform.position = _lerper.Lerp(_initCameraPos, _targetCameraPos, 2);
            return false;
        }
        else if (!_finishedScale)
        {
            _finishedScale = true;
            _mainMenuPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_startButton);
            _lerper.Reset();
        }

        if (_mainMenuCanvas.alpha < 1)
        {
            _mainMenuCanvas.alpha = _lerper.Lerp(0, 1, 0.5f);
            if (_rightMenuBox.alpha < 1)
            {
                _leftMenuBox.alpha = _lerper.Lerp(0, 1, 0.5f);
                _rightMenuBox.alpha = _lerper.Lerp(0, 1, 0.5f);
            }
            return false;
        }

        foreach (User user in SceneManager.Users)
            user.ChangeState("MenuState");

        _lerper.Reset();
        _finishedScale = false;
        EventSystem.current.firstSelectedGameObject = _startButton;
        _uiManager.ChangeEventSystem(1);
        return true;
    }

    public override bool OutroTransition()
    {
        if (_mainMenuCanvas.alpha > 0)
        {
            _menuPage.OutroCurrentPanel(0.5f);
            _mainMenuCanvas.alpha = _lerper.Lerp(1, 0, 0.5f);
            if (_sceneManager.CheckNextScene<JoinGameScene>())
            {
                _leftMenuBox.alpha = _lerper.Lerp(1, 0, 0.5f);
                _rightMenuBox.alpha = _lerper.Lerp(1, 0, 0.5f);
            }
            return false;
        }
        return _menuPage.OutroCurrentPanel(0.5f);
    }

    public override void Cleanup()
    {
        _mainMenuPanel.SetActive(false);
    }

    public override void SceneUpdate()
    {
        _menuPage.Update();
    }
}
