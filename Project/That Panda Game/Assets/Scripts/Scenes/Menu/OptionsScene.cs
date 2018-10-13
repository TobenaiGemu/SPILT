using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsScene : Scene
{
    private GameObject _optionsPanel;
    private GameObject _resolutionButton;
    private CanvasGroup _optionsCanvas;
    private TimeLerper _lerper;
    private float _optionsAlpha;

    public void Awake()
    {
        _optionsPanel = GameObject.Find("Canvas").transform.Find("OptionsPanel").gameObject;
        _resolutionButton = _optionsPanel.transform.Find("Resolution").gameObject;
        _optionsCanvas = _optionsPanel.GetComponent<CanvasGroup>();
        _lerper = new TimeLerper();

        _optionsPanel.SetActive(false);
    }

    public override void Initialize()
    {
        _optionsPanel.SetActive(true);
        EventSystem.current.firstSelectedGameObject = _resolutionButton;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_resolutionButton);
        _lerper.Reset();
    }

    public override void Cleanup()
    {
        _optionsPanel.SetActive(false);
    }

    public override bool IntroTransition()
    {
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
            return false;
        }
        _lerper.Reset();
        return base.OutroTransition();
    }
}
