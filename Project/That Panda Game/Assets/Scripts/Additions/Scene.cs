using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    protected SceneManager _sceneManager;

    public Scene(SceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }

    public virtual void Initialize() { }
    public virtual void Cleanup() { }

    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
