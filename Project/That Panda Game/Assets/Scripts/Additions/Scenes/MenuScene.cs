using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : Scene
{
    public MenuScene(SceneManager sceneManager)
        :base(sceneManager)
    {

    }

    public override void Initialize()
    {
        foreach (User user in SceneManager.Users)
            user.ChangeState("MenuState");
    }

    public override void Update()
    {
        
    }
}
