using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserState
{
    protected Joystick _joystick;
    protected User _user;
    protected SceneManager _sceneManager;

    public UserState(User user)
    {
        _joystick = user.Joystick;
        _user = user;
        _sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }

    public virtual void Initialize()
    {

    }

    public virtual void Cleanup()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }
}
