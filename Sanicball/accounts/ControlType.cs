using System;
using Godot;

namespace Sanicball.Account;

//TODO Add rebindable keymaps
//TODO if deviceID is unique use that instead of the enum
public enum ControlType
{
    //putting joysticks at the start makes the device id line up with the enum
    Joystick1,
    Joystick2,
    Joystick3,
    Joystick4,
    Keyboard 
}

public static class ControlTypeImpl
{
    static ControlTypeImpl()
    {
        if (!InputMap.HasAction("keyboard_left")) { InputMap.AddAction("keyboard_left"); }
        InputMap.ActionAddEvent("keyboard_left", new InputEventKey() { Keycode = Key.A });

        if (!InputMap.HasAction("joy1_left")) { InputMap.AddAction("joy1_left"); }
        InputMap.ActionAddEvent("joy1_left", new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.LeftStick });

        if (!InputMap.HasAction("joy1_down")) { InputMap.AddAction("joy1_down"); }

        if (!InputMap.HasAction("keyboard_ready")) { InputMap.AddAction("keyboard_ready"); }
        InputMap.ActionAddEvent("keyboard_ready", new InputEventKey() { Keycode = Key.R });

        if (!InputMap.HasAction("joy1_ready")) { InputMap.AddAction("joy1_ready"); }
        InputMap.ActionAddEvent("joy1_ready", new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.B });

        if (!InputMap.HasAction("keyboard_confirm")) { InputMap.AddAction("keyboard_confirm"); }
        InputMap.ActionAddEvent("keyboard_confirm", new InputEventKey() { Keycode = Key.Enter });

        if (!InputMap.HasAction("joy1_confirm")) { InputMap.AddAction("joy1_confirm"); }
        InputMap.ActionAddEvent("joy1_confirm", new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.A });
    }

    public static Texture2D Icon(this ControlType controlType)
    {
        return controlType switch
        {
            ControlType.Keyboard => GD.Load<Texture2D>("res://Scenes/S0-Shared/Keyboard.png"),
            ControlType.Joystick1 => GD.Load<Texture2D>("res://Scenes/S0-Shared/Joystick1.png"),
            _ => throw new InvalidCastException($"Invalid control type detected: {controlType}")
        };
    }

    public static Vector3 NormalizedForce(this ControlType control)
    {
        Vector3 force = new();

        switch (control)
        {
            case ControlType.Keyboard:
                if (Input.IsActionPressed("keyboard_up")) { force += Vector3.Forward; }
                if (Input.IsActionPressed("keyboard_down")) { force += Vector3.Back; }
                if (Input.IsActionPressed("keyboard_left")) { force += Vector3.Left; }
                if (Input.IsActionPressed("keyboard_right")) { force += Vector3.Right; }
                break;

            case ControlType.Joystick1:
                force.X -= Input.GetActionStrength("joystick_down");
                force.Z -= Input.GetActionStrength("joystick_left");
                force.Z += Input.GetActionStrength("joystick_right");
                force.X += Input.GetActionStrength("joystick_up");
                break;

            default:
                throw new InvalidCastException($"Invalid control type detected: {control}");
        };

        return force.Normalized();
    }

    public static bool LeftPressed(this ControlType control, InputEvent input)
    {
        return control switch
        {
            ControlType.Keyboard => input.IsActionPressed("keyboard_left"),
            ControlType.Joystick1 => input.IsActionPressed("joystick_left"),
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool RightPressed(this ControlType control, InputEvent input)
    {
        return control switch
        {
            ControlType.Keyboard => input.IsActionPressed("keyboard_right"),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool UpPressed(this ControlType control, InputEvent input)
    {
        return control switch
        {
            ControlType.Keyboard => input.IsActionPressed("keyboard_up"),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool DownPressed(this ControlType control, InputEvent input)
    {
        return control switch
        {
            ControlType.Keyboard => input.IsActionPressed("keyboard_down"),
            ControlType.Joystick1 => input.IsActionPressed("joy1_down"),
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }


    public static bool Confirmed(this ControlType controlType, InputEvent input)
    {
        return controlType switch
        {
            ControlType.Keyboard => input.IsActionPressed("keyboard_confirm"),
            ControlType.Joystick1 => input.IsActionPressed("joy1_confirm"),
            _ => throw new InvalidCastException($"Invalid control type detected: {controlType}"),
        };
    }

    public static bool Ready(this ControlType controlType, InputEvent input)
    {
        return controlType switch
        {
            ControlType.Keyboard => input.IsActionPressed("keyboard_ready"),
            ControlType.Joystick1 => input.IsActionPressed("joy1_ready"),
            _ => throw new InvalidCastException($"Invalid control type detected: {controlType}")
        };
    }
}