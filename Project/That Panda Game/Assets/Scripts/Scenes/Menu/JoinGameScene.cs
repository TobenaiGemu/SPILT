using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameScene : Scene
{
    private GameObject _joinGamePanel;
    private CanvasGroup _joinGameCanvas;
    private TimeLerper _lerper;

    private CanvasGroup _leftMenuBox;
    private CanvasGroup _rightMenuBox;

    void Awake ()
    {
        _joinGamePanel = GameObject.Find("Canvas").transform.Find("JoinGamePanel").gameObject;
        _joinGameCanvas = _joinGamePanel.GetComponent<CanvasGroup>();
        _joinGameCanvas.alpha = 0;

        _leftMenuBox = GameObject.Find("Canvas").transform.Find("Left Menu Box").GetComponent<CanvasGroup>();
        _rightMenuBox = GameObject.Find("Canvas").transform.Find("Right Menu Box").GetComponent<CanvasGroup>();

        _lerper = new TimeLerper();
        _joinGamePanel.SetActive(false);
    }

    public override void Initialize()
    {
        _lerper.Reset();
        for (int i = 0; i < SceneManager.MaxUsers; i++)
        {
            _joinGamePanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "PRESS A TO JOIN!";
        }

        foreach (User user in SceneManager.Users)
            user.SetPlaying(false);

        _joinGamePanel.SetActive(true);
    }

    public override bool IntroTransition()
    {
        if (_leftMenuBox.alpha > 1)
        {
            _leftMenuBox.alpha = _lerper.Lerp(1, 0, 0.5f);
            _rightMenuBox.alpha = _lerper.Lerp(1, 0, 0.5f);
            if (_leftMenuBox.alpha >= 1)
                _lerper.Reset();
            return false;
        }

        if (_joinGameCanvas.alpha != 1)
        {
            _joinGameCanvas.alpha = _lerper.Lerp(0, 1, 0.5f);
            return false;
        }
        foreach (User user in SceneManager.Users)
            user.ChangeState<JoinGameState>();
        _lerper.Reset();
        return true;
    }

    public override bool OutroTransition()
    {
        if (_joinGameCanvas.alpha != 0)
        {
            _joinGameCanvas.alpha = _lerper.Lerp(1, 0, 0.5f);
            return false;
        }
        _lerper.Reset();
        _joinGamePanel.SetActive(false);
        return true;
    }
}
