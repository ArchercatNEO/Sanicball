using System;
using System.Collections.Generic;
using Godot;
using Sanicball.Account;

namespace Sanicball.Scenes;

/// <summary>
/// Manages players logging in/out.
/// The CharacterSelect is then used to confirm the player's character and handle the selector logic
/// </summary>
public partial class CharacterSelectManager : HBoxContainer
{
    [Export] private Node3D playerSpawner = null!;

    // this mess is to ensure CharacterSelect doesn't hold a reference to it's manager
    // Same is true for characterManager -> LobbyManager
    private int _readyPlayers = 0;
    public event EventHandler<int>? OnReadyPlayerChange;
    private void ReadyPlayerCallback(object? sender, bool ready)
    {
        _readyPlayers += ready ? 1 : -1;
        OnReadyPlayerChange?.Invoke(this, _readyPlayers);
    }

    public readonly Dictionary<ControlType, CharacterSelect> activePanels = [];

    public override void _Ready()
    {
        OnDeviceConnected((long)ControlType.Keyboard, true);
    }

    public override void _EnterTree() { Input.JoyConnectionChanged += OnDeviceConnected; }
    public override void _ExitTree() { Input.JoyConnectionChanged -= OnDeviceConnected; }

    private void OnDeviceConnected(long device, bool connected)
    {
        ControlType controller = (ControlType)device;
        if (connected)
        {
            CharacterSelect panel = CharacterSelect.Create(controller, playerSpawner);
            AddChild(panel);
            activePanels.Add(controller, panel);
            panel.OnReadyChanged += ReadyPlayerCallback;
        }
        else
        {
            CharacterSelect panel = activePanels[controller];
            RemoveChild(panel);
            activePanels.Remove(controller);
            panel.QueueFree();
        }
    }
}