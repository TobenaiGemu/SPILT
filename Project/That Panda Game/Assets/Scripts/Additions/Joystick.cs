using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick
{
    private int _controllerId;

    public Joystick(int id)
    {
        _controllerId = id;
    }

    public int GetId()
    {
        return _controllerId;
    }

    public float GetAnalogue1Axis(string axis)
    {
        switch (axis)
        {
            case "Horizontal":
                return Input.GetAxis("Joy" + _controllerId + "Horizontal1");
            case "Vertical":
                return Input.GetAxis("Joy" + _controllerId + "Vertical1");
        }
        return 0;
    }

    public float GetAnalogue2Axis(string axis)
    {
        switch (axis)
        {
            case "Horizontal":
                return Input.GetAxis("Joy" + _controllerId + "Horizontal2");
            case "Vertical":
                return Input.GetAxis("Joy" + _controllerId + "Vertical2");
        }
        return 0;
    }

    public bool WasButtonPressed(string button)
    {
        switch (button)
        {
            case "Button1":
                return Input.GetButtonDown("Joy" + _controllerId + "Button1");
            case "Button2":
                return Input.GetButton("Joy" + _controllerId + "Button2");
            case "Button3":
                return Input.GetButton("Joy" + _controllerId + "Button3");
            case "Button0":
                return Input.GetButton("Joy" + _controllerId + "Button0");
        }
        return false;
    }
}
