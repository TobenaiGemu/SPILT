using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static int MaxUsers { get; private set; }
    public static User[] Users { get; private set; }

    private Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

    private Scene _nextScene;
    private Scene _currentScene;

    private bool _paused;
    private bool _cleanedScene;

    // Use this for initialization
    void Awake()
    {
        MaxUsers = 4;
        _scenes.Add("GameScene", new GameScene(this));
        _scenes.Add("MenuScene", new MenuScene(this));

        Users = new User[MaxUsers];

        for (int i = 0; i < MaxUsers; i++)
        {
            Users[i] = new User(this, i + 1);
        }
        
        ChangeScene("MenuScene");
        _paused = false;
    }

    public void ChangeScene(string sceneName)
    {
        _paused = false;
        _nextScene = _scenes[sceneName];
    }

    public User GetUser(int num)
    {
        if (num < 1)
            return Users[0];
        if (num > MaxUsers)
            return Users[MaxUsers - 1];
        return Users[num - 1];
    }

    public T GetScene<T>(string sceneName)
        where T : Scene
    {
        return _scenes[sceneName] as T;
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
            _currentScene.Update();

        //Completes the outro transition of the current scene before cleaning it and the intro transition of the next scene after initializing it
        if (_nextScene != null)
        {
            if (_currentScene == null || _currentScene.OutroTransition())
            {
                if ((!_cleanedScene && _currentScene != null) || _currentScene == null)
                {
                    if (_currentScene != null)
                        _currentScene.Cleanup();
                    _nextScene.Initialize();
                    _cleanedScene = true;
                }
                if (_nextScene.IntroTransition())
                {
                    _currentScene = _nextScene;
                    _nextScene = null;
                    _cleanedScene = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (_paused)
            return;

        if (_currentScene != null)
            _currentScene.FixedUpdate();
    }
}
