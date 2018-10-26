﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static int MaxUsers { get; private set; }
    public static User[] Users { get; private set; }

    private List<Scene> _scenes = new List<Scene>();

    private GameObject _scenesPool;

    private Scene _nextScene;
    private Scene _currentScene;

    private bool _paused;
    private bool _cleanedScene;

    private bool _initNextScene;

    // Use this for initialization
    void Awake()
    {
        foreach (string name in Input.GetJoystickNames())
            Debug.Log(name);
        _scenesPool = GameObject.Find("Scenes");

        MaxUsers = 4;
        _scenes.Add(_scenesPool.transform.Find("GameScene").GetComponent<GameScene>());
        _scenes.Add(_scenesPool.transform.Find("MainMenuScene").GetComponent<MainMenuScene>());
        _scenes.Add(_scenesPool.transform.Find("GamemodeScene").GetComponent<GamemodeScene>());
        _scenes.Add(_scenesPool.transform.Find("InfoScene").GetComponent<InfoScene>());
        _scenes.Add(_scenesPool.transform.Find("OptionsScene").GetComponent<OptionsScene>());

        foreach (Scene scene in _scenes)
            scene.Setup();

        Users = new User[MaxUsers];

        for (int i = 0; i < MaxUsers; i++)
        {
            Users[i] = new User(this, i + 1);
        }
        
        ChangeScene<MainMenuScene>();
        _paused = false;
        _initNextScene = true;
    }

    public void ChangeScene<T>()
    {
        if (_nextScene != null)
            return;
        _paused = false;

        foreach (Scene scene in _scenes)
        {
            if (scene.GetType() == typeof(T))
            {
                _nextScene = scene;
                break;
            }
        };
    }

    public User GetUser(int num)
    {
        if (num < 1)
            return Users[0];
        if (num > MaxUsers)
            return Users[MaxUsers - 1];
        return Users[num - 1];
    }

    public T GetScene<T>()
        where T : Scene
    {
        foreach (Scene scene in _scenes)
        {
            if (scene.GetType() == typeof(T))
            {
                return scene as T;
            }
        };
        throw new System.Exception("WTF YOU DOIN M8");
    }

    public void PauseScene()
    {
        _paused = true;
    }

    public void ResumeScene()
    {
        _paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_paused)
            return;

        if (_currentScene != null)
            _currentScene.SceneUpdate();

        //Completes the outro transition of the current scene before cleaning it and the intro transition of the next scene after initializing it
        if (_nextScene != null)
        {
            if (_currentScene == null || _currentScene.OutroTransition())
            {
                if ((!_cleanedScene && _currentScene != null) || _currentScene == null)
                {
                    if (_currentScene != null)
                        _currentScene.Cleanup();
                    if (_initNextScene)
                    {
                        _nextScene.Initialize();
                        _initNextScene = false;
                    }
                    
                    _cleanedScene = true;
                    _currentScene = null;
                }
                if (_nextScene.IntroTransition())
                {
                    _currentScene = _nextScene;
                    _nextScene = null;
                    _cleanedScene = false;
                    _initNextScene = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (_paused)
            return;

        if (_currentScene != null)
            _currentScene.SceneFixedUpdate();
    }
}
