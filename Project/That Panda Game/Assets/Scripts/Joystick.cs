using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick
{
    private int _controllerId;
    private string _controllerName;

    public Joystick(int id)
    {
        _controllerId = id;
        if (id <= Input.GetJoystickNames().Length)
            _controllerName = Input.GetJoystickNames()[id - 1];

        if (_controllerName != "Wireless Controller")
        {
            StandaloneInputModule module = GameObject.Find("EventSystem" + _controllerId).GetComponent<StandaloneInputModule>();
            module.horizontalAxis = "xJoy" + _controllerId + "Horizontal1";
            module.verticalAxis = "xJoy" + _controllerId + "Vertical1";
            module.submitButton = "xJoy" + _controllerId + "Button1";
            module.cancelButton = "xJoy" + _controllerId + "Button2";
        }
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
                return Input.GetAxis(((_controllerName == "Wireless Controller")?"":"x") + "Joy" + _controllerId + "Horizontal1");
            case "Vertical":
                return Input.GetAxis(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Vertical1");
        }
        return 0;
    }

    public float GetAnalogue2Axis(string axis)
    {
        switch (axis)
        {
            case "Horizontal":
                return Input.GetAxis(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Horizontal2");
            case "Vertical":
                return Input.GetAxis(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Vertical2");
        }
        return 0;
    }

    public float GetAxis(string axis)
    {
        switch (axis)
        {
            case "L2":
                return Input.GetAxis(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "L2");
            case "R2":
                return Input.GetAxis(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "R2");
        }
        return 0;
    }

    public bool WasButtonPressed(string button)
    {
        switch (button)
        {
            case "Button1":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Button1");
            case "Button2":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Button2");
            case "Button3":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Button3");
            case "Button0":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Button0");
            case "Pause":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Pause");
        }
        return false;
    }
}
