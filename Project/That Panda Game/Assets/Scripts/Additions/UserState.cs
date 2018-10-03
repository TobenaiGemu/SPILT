using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserState
{
    protected Joystick _joystick;

    public UserState(User user)
    {
        _joystick = user.Joystick;
    }

    public virtual void Initialize()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }
}
