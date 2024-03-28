using System;
using System.Collections.Generic;
using Godot;
using Sanicball.Account;

namespace Sanicball.Scenes;

/// <summary>
/// Manages players logging in/out.
/// The CharacterSelect is then used to confirm the player's character and handle the selector logic
/// </summary>
public partial class CharacterSelectManager : Control
{
    private readonly HashSet<ControlType> activeControllers = [];
    
    public override void _UnhandledInput(InputEvent @event)
    {
        foreach (ControlType control in Enum.GetValues(typeof(ControlType)))
        {
            if (!activeControllers.Contains(control) && control.Confirmed())
            {
                activeControllers.Add(control);
                
                CharacterSelect panel = CharacterSelect.Create(control);
                AddChild(panel);
                
                panel.OnPlayerConfirmed += (sender, confirmation) => {
                    activeControllers.Remove(control);
                };
            }
        }
    }
}