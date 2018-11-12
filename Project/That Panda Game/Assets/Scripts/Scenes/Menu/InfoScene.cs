using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoScene : Scene
{
    private GameObject _infoPanel;
    private CanvasGroup _infoCanvas;
    private GameObject _controlsButton;

    private TimeLerper _lerper;
    private float _infoAlpha;

    private MenuPage _menuPage;

    public void Awake()
    {
        _lerper = new TimeLerper();
        _infoPanel = GameObject.Find("Canvas").transform.Find("InfoPanel").gameObject;
        _controlsButton = _infoPanel.transform.Find("Controls").gameObject;
        _infoCanvas = _infoPanel.GetComponent<CanvasGroup>();

        _menuPage = new MenuPage(_infoPanel);
        //Set all the corresponding right panels for each button based on index
        _menuPage.SetRightPanel("ControlsPanel", 0);
        //_menuPage.SetRightPanel("ControlsPanel", 0);
        //_menuPage.SetRightPanel("PowerupsPanel", 1);
        //_menuPage.SetRightPanel("EventsPanel", 2);
        //_menuPage.SetRightPanel("CreditsPanel", 3);

        _infoPanel.SetActive(false);
    }

    public override void Initialize()
    {
        _infoPanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = _controlsButton;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_controlsButton);
        _lerper.Reset();
    }

    public override void Cleanup()
    {
        _infoPanel.SetActive(false);
    }

    public override bool IntroTransition()
    {
        _infoCanvas.alpha = _infoAlpha;
        if (_infoAlpha < 1)
        {
            _infoAlpha = _lerper.Lerp(0, 1, 0.5f);
            return false;
        }
        _lerper.Reset();
        return true;
    }

    public override bool OutroTransition()
    {
        _infoCanvas.alpha = _infoAlpha;
        if (_infoAlpha > 0)
        {
            _infoAlpha = _lerper.Lerp(1, 0, 0.5f);
            return false;
        }
        _lerper.Reset();
        return true;
    }

    public override void SceneUpdate()
    {
        _menuPage.Update();
        base.SceneUpdate();
    }
}
