using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    private UserState _currentState;
    private List<UserState> _states;

    public Joystick Joystick {get; private set;}
    public int UserId { get; private set; }
    public Character AssignedCharacter { get; private set; }
    public bool IsPlaying { get; private set; }

    //The user class has a reference to the Joystick class that handles input, and holds a reference to every state that the user could be in.
    //It also holds a reference to the character assigned to it
    public User(SceneManager sceneManager, int userId)
    {
        UserId = userId;
        Joystick = new Joystick(UserId);
        _states = new List<UserState>();

        _states.Add(new GameState(this));
        _states.Add(new MenuState(this));
        _states.Add(new JoinGameState(this));
        _states.Add(new CharacterSelectState(this));
        _states.Add(new OptionsState(this));
    }

    //Update the current state
    public void Update()
    {
        _currentState.Update();
    }

    public void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    //Change the current UserState
    public void ChangeState<T>()
        where T : UserState
    {
        if (_currentState != null)
            _currentState.Cleanup();
        foreach (UserState state in _states)
        {
            if (state.GetType() == typeof(T))
            {
                _currentState = state;
                break;
            }
        }
        _currentState.Initialize();
    }

    //Attempt to assign a character to this user.
    //If the character is already assigned to a user it cannot be assigned to this
    //This is never called directly, it is only called from a sceneManager instance
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
            AssignedCharacter.Cleanup();
        }
    }

    public void SetPlaying(bool playing)
    {
        IsPlaying = playing;
    }
}
