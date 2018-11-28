using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPage
{
    private List<GameObject> _leftButtons = new List<GameObject>();
    private CanvasGroup[] _rightCanvases;

    private GameObject _canvas;
    private TimeLerper _lerperOutro;
    private TimeLerper _lerperIntro;
    private TimeLerper _lerperCurrentOutro;

    private CanvasGroup _nextRightCanvas;
    private CanvasGroup _curRightCanvas;

    private GameObject _prevSelected;
    private GameObject _curSelected;

    private bool _hasChanged;

    private bool _forceNext;

    public MenuPage(GameObject panel)
    {
        _canvas = GameObject.Find("Canvas");

        for (int i = 0; i < panel.transform.childCount; i++)
        {
            _leftButtons.Add(panel.transform.GetChild(i).gameObject);
        }

        _rightCanvases = new CanvasGroup[_leftButtons.Count];

        _lerperOutro = new TimeLerper();
        _lerperIntro = new TimeLerper();
        _lerperCurrentOutro = new TimeLerper();
    }

    public void Initialize()
    {
        //Reset all values
        _prevSelected = null;
        _curSelected = null;
        _curRightCanvas = null;
        _nextRightCanvas = null;
        _lerperIntro.Reset();
        _lerperOutro.Reset();
        _hasChanged = false;
        _forceNext = false;
    }

    public void SetRightPanel(string name, int btnIndex)
    {
        //Find an object in the canvas with the name and assign it to a button as btnIndex (from top to bottom)
        _rightCanvases[btnIndex] = _canvas.transform.Find(name).GetComponent<CanvasGroup>();
        _rightCanvases[btnIndex].alpha = 0;
    }

    private void ButtonChange(string name)
    {
        //Check if there was a button change
        _hasChanged = true;
        _lerperIntro.Reset();
        _lerperOutro.Reset();

        if (_nextRightCanvas != null)
            _curRightCanvas = _nextRightCanvas;

        //If there was a button change, set the next canvas to the changed button
        for (int i = 0; i < _leftButtons.Count; i++)
        {
            if (_leftButtons[i].name == name)
            {
                _nextRightCanvas = _rightCanvases[i];
                if (_nextRightCanvas != null)
                    _nextRightCanvas.gameObject.SetActive(true);
                return;
            }
        }
    }

    private bool OutroRightPanel(CanvasGroup canvas)
    {
        //Outro the full right panel
        if (canvas == null)
            return true;
        
        if (canvas.alpha > 0)
        {
            canvas.alpha = _lerperOutro.Lerp(1, 0, 0.1f);
            return false;
        }
        canvas.gameObject.SetActive(false);
        _lerperOutro.Reset();
        return true;
    }

    private bool IntroRightPanel(CanvasGroup canvas)
    {
        //Intro the full right panel
        if (canvas == null)
            return true;
        if (canvas.alpha < 1)
        {
            canvas.alpha = _lerperIntro.Lerp(0, 1, 0.1f);
            return false;
        }
        _lerperIntro.Reset();
        return true;
    }

    public bool OutroCurrentPanel(float timeToTransition)
    {
        //Outro the current panel that is within the full panel
        if (_curRightCanvas == null)
            return true;

        if (_curRightCanvas.alpha > 0)
        {
            _curRightCanvas.alpha = _lerperCurrentOutro.Lerp(1, 0, timeToTransition);
            return false;
        }
        _curRightCanvas.alpha = 0;
        _curRightCanvas.gameObject.SetActive(false);
        _lerperCurrentOutro.Reset();
        if (_nextRightCanvas != null)
            _nextRightCanvas.GetComponent<CanvasGroup>().alpha = 0;
        return true;
    }

    public void Update()
    {
        //Check for button changes
        _prevSelected = _curSelected;
        _curSelected = EventSystem.current.currentSelectedGameObject;

        if (_curSelected != null && _curSelected != _prevSelected)
        {
            ButtonChange(_curSelected.name);
        }

        if (_hasChanged)
        {
            if (OutroRightPanel(_curRightCanvas))
            {
                if (IntroRightPanel(_nextRightCanvas))
                {
                    _curRightCanvas = _nextRightCanvas;
                    _nextRightCanvas = null;
                    _hasChanged = false;
                }
            }
        }
    }
}
