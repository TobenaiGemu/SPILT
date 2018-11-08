using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuScene : Scene
{
    private GameObject _mainMenuPanel;
    private CanvasGroup _mainMenuCanvas;
    private GameObject _startButton;

    private GameObject _planet;

    private Vector3 _initCameraPos;
    private Vector3 _targetCameraPos;

    private bool _finishedScale;
    private TimeLerper _lerper;

    private float _alpha;


    public void Awake()
    {
        Camera.main.transform.position = new Vector3(0, 0, -1100);
        _targetCameraPos = new Vector3(0, 0, -100);

        _lerper = new TimeLerper();
        _planet = GameObject.Find("Planet");
        _alpha = 0;

        _mainMenuPanel = GameObject.Find("Canvas").transform.Find("MainMenuPanel").gameObject;
        _mainMenuCanvas = _mainMenuPanel.GetComponent<CanvasGroup>();
        _startButton = _mainMenuPanel.transform.Find("Start").gameObject;
    }

    public override void Initialize()
    {
        _initCameraPos = Camera.main.transform.position;
        _lerper.Reset();
    }

    public override bool IntroTransition()
    {
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

        _mainMenuCanvas.alpha = _alpha;
        if (_alpha < 1)
        {
            _alpha = _lerper.Lerp(0, 1, 0.5f);
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
        _mainMenuCanvas.alpha = _alpha;
        if (_alpha > 0)
        {
            _alpha = _lerper.Lerp(1, 0, 0.5f);
            return false;
        }
        return true;
    }

    public override void Cleanup()
    {
        _mainMenuPanel.SetActive(false);
    }
}
