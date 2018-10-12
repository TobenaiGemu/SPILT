using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup
{
    private GameScene _gameScene;

    public Pickup()
    {
        _gameScene = GameObject.Find("SceneManager").GetComponent<SceneManager>().GetScene<GameScene>("GameScene");
    }

    public virtual void ExecutePickup(Character character)
    {

    }
}
