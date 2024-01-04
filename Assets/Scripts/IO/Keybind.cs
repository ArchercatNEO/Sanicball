using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sanicball.IO
{
    [Serializable]
    public class KeybindCollection
    {
        private static readonly LazySaveFile<KeybindCollection> instance = new("GameKeybinds.json");
        public static KeybindCollection Instance
        {
            get => instance.File;
            set => instance.File = value;
        }

        public static bool GetKey(Keybind key) => Input.GetKey(Instance.keybinds[key]);
        public static string GetKeyBindName(Keybind keybind) => GetKeyCodeName(Instance[keybind]);
        public static string GetKeyCodeName(KeyCode kc)
        {
            string str = kc.ToString();
            //Insert spaces before capital letters
            for (int i = 0; i < str.Length; i++)
            {
                if (i != 0 && !char.IsLower(str[i]) && char.IsLower(str[i - 1]))
                {
                    str = str[..i] + " " + str[i..];
                    //Increment i to account for the newly inserted space
                    i++;
                }
            }

            return str;
        }

        public KeyCode this[Keybind b]
        {
            get { return keybinds[b]; }
            set { keybinds[b] = value; }
        }

        public Dictionary<Keybind, KeyCode> keybinds = new() {
            {Keybind.Forward, KeyCode.W},
            {Keybind.Left, KeyCode.A},
            {Keybind.Back, KeyCode.S},
            {Keybind.Right, KeyCode.D},

            {Keybind.CameraUp, KeyCode.UpArrow},
            {Keybind.CameraLeft, KeyCode.LeftArrow},
            {Keybind.CameraDown, KeyCode.DownArrow},
            {Keybind.CameraRight, KeyCode.RightArrow},

            {Keybind.Brake, KeyCode.LeftShift},
            {Keybind.Jump, KeyCode.Space},
            {Keybind.Respawn, KeyCode.R},
            {Keybind.Menu, KeyCode.Return},
            {Keybind.NextSong, KeyCode.N},
            {Keybind.Chat, KeyCode.T}

            /*  Joystick one
             * {KeyBind.joy1brake, KeyCode.Joystick1Button1} // x + a
             * {KeyBind.joy1jump, KeyCode.Joystick1Button0} // o + b
             * {KeyBind.joy1respawn, KeyCode.Joystick1Button3} // delta + y
             * {KeyBind.joy1menu, KeyCode.Joystick1Button2} // square + x*
            */
        };
    }

    //TODO: use this enum for keybinds
    public enum Keybind
    {
        Forward,
        Left,
        Back,
        Right,

        CameraUp,
        CameraLeft,
        CameraDown,
        CameraRight,

        Brake,
        Jump,
        Respawn, //Toggle ready in lobby, respawn ingame
        Menu,
        NextSong,
        Chat
    }
}