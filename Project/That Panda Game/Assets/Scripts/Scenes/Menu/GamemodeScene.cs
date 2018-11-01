using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamemodeScene : Scene
{
    private GameObject _gamemodePanel;
    private CanvasGroup _gamemodeCanvas;
    private float _gamemodeAlpha;
    private TimeLerper _lerper;

    private GameObject _practiceButton;

    private MenuPage _menuPage;

    public void Awake()
    {
        _lerper = new TimeLerper();
        _gamemodePanel = GameObject.Find("Canvas").transform.Find("GamemodePanel").gameObject;
        _practiceButton = _gamemodePanel.transform.Find("Practice").gameObject;
        _gamemodeCanvas = _gamemodePanel.GetComponent<CanvasGroup>();

        _menuPage = new MenuPage(_gamemodePanel);
        _menuPage.SetRightPanel("PracticePanel", 0);
        _menuPage.SetRightPanel("GameMode1Panel", 1);
    }

    public override void Initialize()
    {
        _gamemodePanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = _practiceButton;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_practiceButton);
        base.Initialize();
    }

    public override bool IntroTransition()
    {
        _gamemodeCanvas.alpha = _gamemodeAlpha;
        if (_gamemodeAlpha < 1)
        {
            _gamemodeAlpha = _lerper.Lerp(0, 1, 0.5f);
            return false;
        }
        _lerper.Reset();
        return true;
    }

    public override bool OutroTransition()
    {
        _gamemodeCanvas.alpha = _gamemodeAlpha;
        if (_gamemodeAlpha > 0)
        {
            _gamemodeAlpha = _lerper.Lerp(1, 0, 0.5f);
            _menuPage.OutroCurrentPanel(0.5f);
            return false;
        }

        if (!_menuPage.OutroCurrentPanel(0.5f))
            return false;
        _lerper.Reset();
        _gamemodePanel.SetActive(false);
        return true;
    }

    public override void SceneUpdate()
    {
        _menuPage.Update();
    }
}
