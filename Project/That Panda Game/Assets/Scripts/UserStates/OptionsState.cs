using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsState : UserState
{
    private bool _updateWait;

	public OptionsState(User user)
        :base(user)
    {
    }

    public override void Initialize()
    {
        _updateWait = true;
    }

    public override void Update()
    {
        if (_updateWait)
        {
            _updateWait = false;
            return;
        }
        if (_joystick.WasButtonPressed("Button2"))
        {
            _sceneManager.GetScene<OptionsScene>().GotoLeftButtons();
        }
        if (_joystick.WasButtonPressed("Button1") && EventSystem.current.currentSelectedGameObject.name =="Slider")
        {
            _sceneManager.GetScene<OptionsScene>().GotoLeftButtons();
        }
    }
}
