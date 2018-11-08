using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : UserState
{
    private MainMenuScene _scene;

    public MenuState(User user, SceneManager sceneManager)
        :base(user)
    {
        _scene = sceneManager.GetScene<MainMenuScene>();
    }

    public override void Initialize()
    {
        _user.SetPlaying(false);
    }

    public override void Update()
    {

    }
}
