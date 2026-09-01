using Godot;
using System;

public partial class InteractableObject : Node3D
{
	[Export(PropertyHint.MultilineText)] protected string defaultMessage;
	[Export] public string animationMeta; // used by main character animator
    public bool interactSuccess = false;


    public virtual void TriggerGizmo(PCStateMachine pc)
    {
        interactSuccess = true;
        GD.Print("pizza");
    }

    public virtual bool ManageInput(InputEvent @event) // returns true if the player control should exit interact state
    {
        return true;
    }


}
