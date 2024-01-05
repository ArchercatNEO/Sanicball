using System;
using Godot;

namespace Sanicball.Account;

public enum ControlType
{
    Keyboard,
    Joystick1
}

public static class ControlTypeImpl
{
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
}