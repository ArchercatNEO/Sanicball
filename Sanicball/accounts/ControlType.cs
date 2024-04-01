using System;
using Godot;

namespace Sanicball.Account;

public enum ControlType
{
    Keyboard,
    Joystick1
}

//TODO Add rebindable keymaps
public static class ControlTypeImpl
{
    public static bool LeftPressed(this ControlType control)
    {
        return control switch
        {
            ControlType.Keyboard => Input.IsActionJustPressed("keyboard_left"),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool RightPressed(this ControlType control)
    {
        return control switch
        {
            ControlType.Keyboard => Input.IsActionJustPressed("keyboard_right"),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool UpPressed(this ControlType control)
    {
        return control switch
        {
            ControlType.Keyboard => Input.IsActionJustPressed("keyboard_up"),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool DownPressed(this ControlType control)
    {
        return control switch
        {
            ControlType.Keyboard => Input.IsActionJustPressed("keyboard_down"),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static Vector3 NormalizedForce(this ControlType control)
    {
        Vector3 force = new();

        switch (control)
        {
            case ControlType.Keyboard:
                if (Input.IsActionPressed("keyboard_down")) { force.X -= 1; }
                if (Input.IsActionPressed("keyboard_left")) { force.Z -= 1; }
                if (Input.IsActionPressed("keyboard_right")) { force.Z += 1; }
                if (Input.IsActionPressed("keyboard_up")) { force.X += 1; }
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

    public static bool Confirmed(this ControlType controlType)
    {
        return controlType switch
        {
            ControlType.Keyboard => Input.IsKeyPressed(Key.Enter),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {controlType}"),
        };
    }
}