using System;
using UnityEngine;
using Sanicball.IO;

namespace Sanicball.Data
{
    /// <summary>
    /// Serves two uses: 
    /// 1. Used as a key to discriminate players on the same client
    /// 2. Used to take actions based on keyboard/joystick combinations
    /// </summary>
    public enum ControlType
    {
        None = -1,
        Keyboard = 0,
        Joystick1 = 1,
        Joystick2 = 2,
        Joystick3 = 3,
        Joystick4 = 4
    }

    public static class ControlTypeImpl
    {
        public static bool KeyboardEnabled = true;
        
        public static string AsName(this ControlType ctrlType)
        {
            return ctrlType switch
            {
                ControlType.None => "",
                ControlType.Keyboard => "Keyboard",
                ControlType.Joystick1 => "Joystick #1",
                ControlType.Joystick2 => "Joystick #2",
                ControlType.Joystick3 => "Joystick #3",
                ControlType.Joystick4 => "Joystick #4",
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        private static string FormLeft(int id, string axis) => $"joy{id}-l{axis}";
        public static Vector3 MovementVector(this ControlType ctrlType)
        {
            return ctrlType switch
            {
                ControlType.None => Vector3.zero,
                ControlType.Keyboard => new Func<Vector3>(() => {
                    if (!KeyboardEnabled) return Vector3.zero;

                    Vector3 output = Vector3.zero;
                    if (KeybindCollection.GetKey(Keybind.Right)) output.x += 1;
                    if (KeybindCollection.GetKey(Keybind.Left)) output.x -= 1;
                    if (KeybindCollection.GetKey(Keybind.Forward)) output.z += 1;
                    if (KeybindCollection.GetKey(Keybind.Back)) output.z -= 1;
                    return output;
                })(),
                ControlType.Joystick1 => new Vector3(Input.GetAxis(FormLeft(1, "x")), 0, Input.GetAxis(FormLeft(1, "y"))),
                ControlType.Joystick2 => new Vector3(Input.GetAxis(FormLeft(2, "x")), 0, Input.GetAxis(FormLeft(2, "y"))),
                ControlType.Joystick3 => new Vector3(Input.GetAxis(FormLeft(3, "x")), 0, Input.GetAxis(FormLeft(3, "y"))),
                ControlType.Joystick4 => new Vector3(Input.GetAxis(FormLeft(4, "x")), 0, Input.GetAxis(FormLeft(4, "y"))),
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        private static string FormRight(int id, string axis) => $"joy{id}-r{axis}";
        public static Vector2 CameraVector(this ControlType ctrlType)
        {
            return ctrlType switch
            {
                ControlType.None => Vector3.zero,
                ControlType.Keyboard => new Func<Vector3>(() => {
                    if (!KeyboardEnabled) { return Vector2.zero; }
                    
                    Vector2 output = Vector2.zero;
                    if (KeybindCollection.GetKey(Keybind.CameraRight)) { output.x += 1; }
                    if (KeybindCollection.GetKey(Keybind.CameraLeft)) { output.x -= 1; }
                    if (KeybindCollection.GetKey(Keybind.CameraUp)) { output.y += 1; }
                    if (KeybindCollection.GetKey(Keybind.CameraDown)) { output.y -= 1; }
                    
                    return output;
                })(),
                ControlType.Joystick1 => new Vector2(Input.GetAxis(FormRight(1, "x")), Input.GetAxis(FormRight(1, "y"))),
                ControlType.Joystick2 => new Vector2(Input.GetAxis(FormRight(2, "x")), Input.GetAxis(FormRight(2, "y"))),
                ControlType.Joystick3 => new Vector2(Input.GetAxis(FormRight(3, "x")), Input.GetAxis(FormRight(3, "y"))),
                ControlType.Joystick4 => new Vector2(Input.GetAxis(FormRight(4, "x")), Input.GetAxis(FormRight(4, "y"))),
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        private static string FormAxis(int id, string axis) => $"joy{id}-d{axis}"; 
        public static void UIMoves(this ControlType ctrlType, out bool up, out bool down, out bool left, out bool right)
        {
            up = ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => Input.GetKey(KeyCode.UpArrow),
                ControlType.Joystick1 => Input.GetAxis(FormAxis(1, "y")) == 1f,
                ControlType.Joystick2 => Input.GetAxis(FormAxis(2, "y")) == 1f,
                ControlType.Joystick3 => Input.GetAxis(FormAxis(3, "y")) == 1f,
                ControlType.Joystick4 => Input.GetAxis(FormAxis(4, "y")) == 1f,
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };

            down = ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => Input.GetKey(KeyCode.DownArrow),
                ControlType.Joystick1 => Input.GetAxis(FormAxis(1, "y")) == -1f,
                ControlType.Joystick2 => Input.GetAxis(FormAxis(2, "y")) == -1f,
                ControlType.Joystick3 => Input.GetAxis(FormAxis(3, "y")) == -1f,
                ControlType.Joystick4 => Input.GetAxis(FormAxis(4, "y")) == -1f,
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };

            left = ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => Input.GetKey(KeyCode.LeftArrow),
                ControlType.Joystick1 => Input.GetAxis(FormAxis(1, "x")) == -1f,
                ControlType.Joystick2 => Input.GetAxis(FormAxis(2, "x")) == -1f,
                ControlType.Joystick3 => Input.GetAxis(FormAxis(3, "x")) == -1f,
                ControlType.Joystick4 => Input.GetAxis(FormAxis(4, "x")) == -1f,
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };

            right = ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => Input.GetKey(KeyCode.RightArrow),
                ControlType.Joystick1 => Input.GetAxis(FormAxis(1, "x")) == 1f,
                ControlType.Joystick2 => Input.GetAxis(FormAxis(2, "x")) == 1f,
                ControlType.Joystick3 => Input.GetAxis(FormAxis(3, "x")) == 1f,
                ControlType.Joystick4 => Input.GetAxis(FormAxis(4, "x")) == 1f,
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        public static bool IsBraking(this ControlType ctrlType)
        {
            return ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => KeybindCollection.GetKey(Keybind.Brake) && KeyboardEnabled,
                ControlType.Joystick1 => Input.GetKey(KeyCode.Joystick1Button1),
                ControlType.Joystick2 => Input.GetKey(KeyCode.Joystick2Button1),
                ControlType.Joystick3 => Input.GetKey(KeyCode.Joystick3Button1),
                ControlType.Joystick4 => Input.GetKey(KeyCode.Joystick4Button1),
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        public static bool IsJumping(this ControlType ctrlType)
        {
            return ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => KeybindCollection.GetKey(Keybind.Jump) && KeyboardEnabled,
                ControlType.Joystick1 => Input.GetKeyDown(KeyCode.Joystick1Button0),
                ControlType.Joystick2 => Input.GetKeyDown(KeyCode.Joystick2Button0),
                ControlType.Joystick3 => Input.GetKeyDown(KeyCode.Joystick3Button0),
                ControlType.Joystick4 => Input.GetKeyDown(KeyCode.Joystick4Button0),
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        public static bool IsRespawning(this ControlType ctrlType)
        {
            return ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => KeybindCollection.GetKey(Keybind.Respawn) && KeyboardEnabled,
                ControlType.Joystick1 => Input.GetKeyDown(KeyCode.Joystick1Button3),
                ControlType.Joystick2 => Input.GetKeyDown(KeyCode.Joystick2Button3),
                ControlType.Joystick3 => Input.GetKeyDown(KeyCode.Joystick3Button3),
                ControlType.Joystick4 => Input.GetKeyDown(KeyCode.Joystick4Button3),
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        public static bool IsOpeningMenu(this ControlType ctrlType)
        {
            return ctrlType switch
            {
                ControlType.None => false,
                ControlType.Keyboard => KeybindCollection.GetKey(Keybind.Menu) && KeyboardEnabled,
                ControlType.Joystick1 => Input.GetKeyDown(KeyCode.Joystick1Button2),
                ControlType.Joystick2 => Input.GetKeyDown(KeyCode.Joystick2Button2),
                ControlType.Joystick3 => Input.GetKeyDown(KeyCode.Joystick3Button2),
                ControlType.Joystick4 => Input.GetKeyDown(KeyCode.Joystick4Button2),
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        public static bool AnyChangingSong()
        {
            foreach (ControlType controlType in Enum.GetValues(typeof(ControlType)))
            {
                if (controlType.IsChangingSong()) { return true; }
            }
            return false;
        }

        public static bool IsChangingSong(this ControlType control)
        {
            return control switch
            {
                ControlType.None => false,
                ControlType.Keyboard => KeybindCollection.GetKey(Keybind.NextSong) && KeyboardEnabled,
                ControlType.Joystick1 => Input.GetKeyDown(KeyCode.JoystickButton6),
                ControlType.Joystick2 => Input.GetKeyDown(KeyCode.JoystickButton6),
                ControlType.Joystick3 => Input.GetKeyDown(KeyCode.JoystickButton6),
                ControlType.Joystick4 => Input.GetKeyDown(KeyCode.JoystickButton6),
                _ => throw new InvalidCastException("Interger has been cast to unknown ControlType variant"),
            };
        }

        public static bool IsOpeningChat()
        {
            return KeybindCollection.GetKey(Keybind.Chat) && KeyboardEnabled;
        }
    }
}