using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static int MaxUsers { get; private set; }
    public static User[] Users { get; private set; }

    private Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();
    private Scene _currentScene;

    // Use this for initialization
    void Awake()
    {
        MaxUsers = 1;
        _scenes.Add("GameScene", new GameScene(this));

        Users = new User[MaxUsers];

        for (int i = 0; i < MaxUsers; i++)
        {
            Users[i] = new User(this, i + 1);
        }
        
        ChangeScene("GameScene");
    }

    public void ChangeScene(string sceneName)
    {
        if (_currentScene != null)
            _currentScene.Cleanup();
        _currentScene = _scenes[sceneName];
        _currentScene.Initialize();
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

    // Update is called once per frame
    void Update()
    {
        _currentScene.Update();
    }

    void FixedUpdate()
    {
        _currentScene.FixedUpdate();
    }
}
