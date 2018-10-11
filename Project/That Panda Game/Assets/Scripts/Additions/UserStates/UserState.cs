using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserState
{
    protected Joystick _joystick;
    protected User _user;

    public UserState(User user)
    {
        _joystick = user.Joystick;
        _user = user;
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
