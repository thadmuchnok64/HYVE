using Godot;
using System;

public partial class InteractableObject : Node3D
{

    [Export] protected string failureMessage;
    public virtual void TriggerGizmo(PCStateMachine pc)
    {
        GD.Print("pizza");
    }

    public virtual bool ManageInput(InputEvent @event) // returns true if the player control should exit interact state
    {
        return true;
    }


}
