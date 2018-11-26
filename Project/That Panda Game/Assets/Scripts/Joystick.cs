using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick
{
    private int _controllerId;
    private string _controllerName;

    //Assign this joystick with an id that is used to map input to a specific connected controller
    public Joystick(int id)
    {
        _controllerId = id;
        if (id <= Input.GetJoystickNames().Length)
            _controllerName = Input.GetJoystickNames()[id - 1];

        //Get a different entry from the Unity input manager depending on the type of controller connected and map it to the InputModule for control of UI elements
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


    //Wrap unity input manager for individual controllers with simple functions
    //Gets a different entry from the input manager depending on the type of controller (x for xbox and non x for ps4 controller)
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
            case "L1":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "L1");
            case "R1":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "R1");
            case "Pause":
                return Input.GetButtonDown(((_controllerName == "Wireless Controller") ? "" : "x") + "Joy" + _controllerId + "Pause");
        }
        return false;
    }
}
