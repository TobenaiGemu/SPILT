using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : Scene
{
    private const int MAX_PLAYERS = 1;

    private SceneManager _sceneManager;

    public GameObject Player { get; private set; }
    public GameObject Planet { get; private set; }

    public GameScene(SceneManager sceneManager)
        :base(sceneManager)
    {
        _sceneManager = sceneManager;
    }

    public override void Initialize()
    {
        Player = GameObject.Find("Player");
        Planet = GameObject.Find("Planet");
        foreach (User user in SceneManager.Users)
            user.ChangeState("PlayerState");
    }

    public override void Update()
    {
        foreach (User user in SceneManager.Users)
        {
            if (user != null)
                user.Update();
        }
    }
}
