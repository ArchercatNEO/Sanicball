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
    
    private readonly HashSet<ControlType> activeControllers = [];
    
    public override void _UnhandledInput(InputEvent @event)
    {
        foreach (ControlType control in Enum.GetValues(typeof(ControlType)))
        {
            if (activeControllers.Contains(control)) { continue; }
            if (!control.Confirmed()) { continue; }
            
            activeControllers.Add(control);
            
            CharacterSelect panel = CharacterSelect.Create(control, playerSpawner);
            AddChild(panel);
            
            panel.TreeExited += () => {
                activeControllers.Remove(control);
            };
        }
    }
}