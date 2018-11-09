using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    protected static SceneManager _sceneManager;
    protected static UiManager _uiManager;

    public virtual void Initialize() { }
    public virtual void Cleanup() { }

    public virtual bool IntroTransition() { return true; }

    public virtual bool OutroTransition() { return true; }

    public void Setup()
    {
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        _uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
    }

    public virtual void SceneUpdate()
    {
        foreach (User user in SceneManager.Users)
        {
            if (user != null)
                user.Update();
        }
    }
    public virtual void SceneFixedUpdate()
    {
        foreach (User user in SceneManager.Users)
        {
            if (user != null)
                user.FixedUpdate();
        }
    }

    public void PauseScene()
    {

    }
    public void PlayScene()
    {

    }
}
