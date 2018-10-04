using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    private UserState _currentState;
    private Dictionary<string, UserState> _states;

    public Joystick Joystick {get; private set;}
    public int UserId { get; private set; }

    public User(SceneManager sceneManager, int userId)
    {
        UserId = userId;
        Joystick = new Joystick(UserId);
        _states = new Dictionary<string, UserState>();

        _states.Add("PlayerState", new PlayerState(sceneManager.GetScene<GameScene>("GameScene"), this, false));
    }

    public void Update()
    {
        _currentState.Update();
    }

    public void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void ChangeState(string state)
    {
        _currentState = _states[state];
        _currentState.Initialize();
    }
}
