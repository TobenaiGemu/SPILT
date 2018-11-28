using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsScene : Scene
{
    private GameObject _optionsPanel;
    private GameObject _resolutionButton;
    private GameObject _volumeButton;
    private CanvasGroup _optionsCanvas;
    private TimeLerper _lerper;
    private float _optionsAlpha;

    private GameObject _resolutionPanel;
    private GameObject _volumePanel;

    private MenuPage _menuPage;

    private GameObject _res1;
    public GameObject _volSlider;
    private bool _updateMenuPage;

    public void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");

        _optionsPanel = canvas.transform.Find("OptionsPanel").gameObject;
        _optionsCanvas = _optionsPanel.GetComponent<CanvasGroup>();

        //Find right panels corresponding to buttons
        _resolutionButton = _optionsPanel.transform.Find("Resolution").gameObject;
        _volumeButton = _optionsPanel.transform.Find("Volume").gameObject;
        _resolutionPanel = canvas.transform.Find("ResolutionPanel").gameObject;
        _volumePanel = canvas.transform.Find("VolumePanel").gameObject;

        _lerper = new TimeLerper();

        _menuPage = new MenuPage(_optionsPanel);

        //Set the right panel to the corresponding button index
        _menuPage.SetRightPanel("ResolutionPanel", 0);
        _menuPage.SetRightPanel("VolumePanel", 1);
        _res1 = GameObject.Find("Canvas").transform.Find("ResolutionPanel").transform.Find("Res1").gameObject;
        _volSlider = GameObject.Find("Canvas").transform.Find("VolumePanel").transform.Find("Slider").gameObject;
        _optionsPanel.SetActive(false);
    }

    public override void Initialize()
    {
        _uiManager.DontPlayFirstHighlightSound();
        _optionsPanel.SetActive(true);
        if (EventSystem.current.currentSelectedGameObject == _volSlider)
        {
            EventSystem.current.firstSelectedGameObject = _volumeButton;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_volumeButton);
        }
        else
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject);
            EventSystem.current.firstSelectedGameObject = _resolutionButton;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_resolutionButton);
        }
        _updateMenuPage = true;
        _menuPage.Initialize();
        for (int i = 1; i <= 4; i++)
        {
            _resolutionPanel.transform.Find("Res" + i).gameObject.SetActive(false);
        }
        _lerper.Reset();
    }

    public void GotoResolution()
    {
        for (int i = 1; i <= 4; i++)
        {
            _resolutionPanel.transform.Find("Res" + i).gameObject.SetActive(true);
        }
        _volumePanel.GetComponent<CanvasGroup>().alpha = 0;
        EventSystem.current.firstSelectedGameObject = _res1;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_res1);
        SceneManager.Users[0].ChangeState<OptionsState>();
        _updateMenuPage = false;
    }

    public void GotoVolume()
    {
        _resolutionPanel.GetComponent<CanvasGroup>().alpha = 0;
        EventSystem.current.firstSelectedGameObject = _volSlider;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_volSlider);
        SceneManager.Users[0].ChangeState<OptionsState>();
        _updateMenuPage = false;
    }

    public void GotoLeftButtons()
    {
        SceneManager.Users[0].ChangeState<MenuState>();
        Initialize();
    }

    public override void Cleanup()
    {
        _optionsPanel.SetActive(false);
    }

    public override bool IntroTransition()
    {
        _menuPage.Update();
        _optionsCanvas.alpha = _optionsAlpha;
        if (_optionsAlpha < 1)
        {
            _optionsAlpha = _lerper.Lerp(0, 1, 0.5f);
            return false;
        }
        _lerper.Reset();
        return true;
    }

    public override bool OutroTransition()
    {
        _optionsCanvas.alpha = _optionsAlpha;
        if (_optionsAlpha > 0)
        {
            _optionsAlpha = _lerper.Lerp(1, 0, 0.5f);
            _menuPage.OutroCurrentPanel(0.5f);
            return false;
        }
        if (!_menuPage.OutroCurrentPanel(0.5f))
            return false;
        _lerper.Reset();
        return base.OutroTransition();
    }

    public override void SceneUpdate()
    {
        if (_updateMenuPage)
            _menuPage.Update();
        base.SceneUpdate();
    }
}
