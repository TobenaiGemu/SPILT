using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : UserState
{
    private MenuScene _scene;

    public MenuState(User user, MenuScene menuScene)
        :base(user)
    {
        _scene = menuScene;
    }

    public override void Initialize()
    {
        Debug.Log("MenuState");
    }

    public override void Update()
    {

    }
}
