using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScene : Scene
{
    private GameObject _characterSelectPanel;
    private CanvasGroup _characterSelectCanvas;
    private TimeLerper _lerper;

    private void Awake()
    {
        _characterSelectPanel = GameObject.Find("Canvas").transform.Find("CharacterSelectPanel").gameObject;
        _characterSelectCanvas = _characterSelectPanel.GetComponent<CanvasGroup>();
        _lerper = new TimeLerper();
        _characterSelectPanel.SetActive(false);
    }

    public override void Initialize()
    {
        _lerper.Reset();
        _characterSelectPanel.SetActive(true);
    }

    public override bool IntroTransition()
    {
        if (_characterSelectCanvas.alpha != 1)
        {
            _characterSelectCanvas.alpha = _lerper.Lerp(0, 1, 0.5f);
            return false;
        }
        return true;
    }


}
