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
        _nextScene = _scenes[sceneName];
        _nextScene.Initialize();
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
        if (_nextScene != null)
        {
            if (_currentScene == null || _currentScene.OutroTransition())
            {
                if (_currentScene != null)
                    _currentScene.Cleanup();
                if (_nextScene.IntroTransition())
                {
                    _currentScene = _nextScene;
                    _nextScene = null;
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
