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

    private Vector3 _planetInitScale;
    private Vector3 _planetEndScale;
    private bool _finishedScale;
    private TimeLerper _lerper;

    private float _alpha;


    public void Awake()
    {
        _lerper = new TimeLerper();
        _planet = GameObject.Find("Planet");
        _planetEndScale = new Vector3(20, 20, 20);
        _alpha = 0;

        _mainMenuPanel = GameObject.Find("Canvas").transform.Find("MainMenuPanel").gameObject;
        _mainMenuCanvas = _mainMenuPanel.GetComponent<CanvasGroup>();
        _startButton = _mainMenuPanel.transform.Find("Start").gameObject;
    }

    public override bool IntroTransition()
    {
        if (_planet.transform.localScale != _planetEndScale)
        {
            _planet.transform.localScale = _lerper.Lerp(_planetInitScale, _planetEndScale, 1);
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

    public override void Initialize()
    {
        _lerper.Reset();
        EventSystem.current.firstSelectedGameObject = _startButton;
        _planetInitScale = _planet.transform.localScale;
    }

    public override void Cleanup()
    {
        _mainMenuPanel.SetActive(false);
    }
}
