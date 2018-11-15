using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    private UserState _currentState;
    private Dictionary<string, UserState> _states;

    public Joystick Joystick {get; private set;}
    public int UserId { get; private set; }
    public Character AssignedCharacter { get; private set; }
    public bool IsPlaying { get; private set; }

    public User(SceneManager sceneManager, int userId)
    {
        UserId = userId;
        Joystick = new Joystick(UserId);
        _states = new Dictionary<string, UserState>();

        _states.Add("GameState", new GameState(this, sceneManager));
        _states.Add("MenuState", new MenuState(this, sceneManager));
        _states.Add("JoinGameState", new JoinGameState(this, sceneManager));
        _states.Add("CharacterSelectState", new CharacterSelectState(this, sceneManager));
        _states.Add("OptionsState", new OptionsState(this, sceneManager));
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
        if (_currentState != null)
            _currentState.Cleanup();
        _currentState = _states[state];
        _currentState.Initialize();
    }

    public bool AttemptAssignCharacter(Character character)
    {
        bool isOk = character.AttemptAssignToUser(this);
        if (isOk)
        {
            AssignedCharacter = character;
            return true;
        }
        return false;
    }

    public void UnassignCharacter()
    {
        if (AssignedCharacter != null)
        {
            Debug.Log(AssignedCharacter.name);
            AssignedCharacter.Cleanup();
        }
    }

    public void SetPlaying(bool playing)
    {
        IsPlaying = playing;
    }
}
