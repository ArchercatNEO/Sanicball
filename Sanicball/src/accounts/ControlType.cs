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
    private static void AddActionIfMissing(StringName action, InputEvent trigger)
    {
        InputMap instance = InputMap.Singleton;
        if (!instance.HasAction(action)) { instance.AddAction(action); }
        instance.ActionAddEvent(action, trigger);
    }

    static ControlTypeImpl()
    {
        AddActionIfMissing(new StringName("keyboard_left"), new InputEventKey() { Keycode = Key.A });
        AddActionIfMissing(new StringName("keyboard_right"), new InputEventKey() { Keycode = Key.D });
        AddActionIfMissing(new StringName("keyboard_up"), new InputEventKey() { Keycode = Key.W });
        AddActionIfMissing(new StringName("keyboard_down"), new InputEventKey() { Keycode = Key.S });
        AddActionIfMissing(new StringName("keyboard_ready"), new InputEventKey() { Keycode = Key.R });
        AddActionIfMissing(new StringName("keyboard_confirm"), new InputEventKey() { Keycode = Key.Enter });

        AddActionIfMissing(new StringName("joy1_left"), new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.LeftStick });
        AddActionIfMissing(new StringName("joy1_right"), new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.LeftStick });
        AddActionIfMissing(new StringName("joy1_up"), new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.LeftStick });
        AddActionIfMissing(new StringName("joy1_down"), new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.LeftStick });
        AddActionIfMissing(new StringName("joy1_ready"), new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.B });
        AddActionIfMissing(new StringName("joy1_confirm"), new InputEventJoypadButton() { Device = 0, ButtonIndex = JoyButton.A });
    }

    public static Texture2D Icon(this ControlType controlType)
    {
        return controlType switch
        {
            ControlType.Keyboard => GD.Load<Texture2D>("res://scenes/S0-Shared/Keyboard.png"),
            ControlType.Joystick1 => GD.Load<Texture2D>("res://scenes/S0-Shared/Joystick1.png"),
            _ => throw new InvalidCastException($"Invalid control type detected: {controlType}")
        };
    }

    public static Vector3 NormalizedForce(this ControlType control)
    {
        Vector3 force = new();
        Input instance = Input.Singleton;

        switch (control)
        {
            case ControlType.Keyboard:
                if (instance.IsActionPressed(new StringName("keyboard_left"))) { force += Vector3.Left; }
                if (instance.IsActionPressed(new StringName("keyboard_right"))) { force += Vector3.Right; }
                if (instance.IsActionPressed(new StringName("keyboard_up"))) { force += Vector3.Forward; }
                if (instance.IsActionPressed(new StringName("keyboard_down"))) { force += Vector3.Back; }
                break;

            case ControlType.Joystick1:
                force.Z -= instance.GetActionStrength(new StringName("joystick_left"));
                force.Z += instance.GetActionStrength(new StringName("joystick_right"));
                force.X += instance.GetActionStrength(new StringName("joystick_up"));
                force.X -= instance.GetActionStrength(new StringName("joystick_down"));
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
            ControlType.Keyboard => input.IsActionPressed(new StringName("keyboard_left")),
            ControlType.Joystick1 => input.IsActionPressed(new StringName("joystick_left")),
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool RightPressed(this ControlType control, InputEvent input)
    {
        return control switch
        {
            ControlType.Keyboard => input.IsActionPressed(new StringName("keyboard_right")),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool UpPressed(this ControlType control, InputEvent input)
    {
        return control switch
        {
            ControlType.Keyboard => input.IsActionPressed(new StringName("keyboard_up")),
            ControlType.Joystick1 => false,
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }

    public static bool DownPressed(this ControlType control, InputEvent input)
    {
        return control switch
        {
            ControlType.Keyboard => input.IsActionPressed(new StringName("keyboard_down")),
            ControlType.Joystick1 => input.IsActionPressed(new StringName("joy1_down")),
            _ => throw new InvalidCastException($"Invalid control type detected: {control}")
        };
    }


    public static bool Confirmed(this ControlType controlType, InputEvent input)
    {
        return controlType switch
        {
            ControlType.Keyboard => input.IsActionPressed(new StringName("keyboard_confirm")),
            ControlType.Joystick1 => input.IsActionPressed(new StringName("joy1_confirm")),
            _ => throw new InvalidCastException($"Invalid control type detected: {controlType}"),
        };
    }

    public static bool Ready(this ControlType controlType, InputEvent input)
    {
        return controlType switch
        {
            ControlType.Keyboard => input.IsActionPressed(new StringName("keyboard_ready")),
            ControlType.Joystick1 => input.IsActionPressed(new StringName("joy1_ready")),
            _ => throw new InvalidCastException($"Invalid control type detected: {controlType}")
        };
    }
}
