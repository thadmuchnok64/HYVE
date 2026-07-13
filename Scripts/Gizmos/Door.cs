using Godot;
using System;

public partial class Door : InteractableObject
{
    [Export] Node3D playerWarpPosition;
    public override void TriggerGizmo(PCStateMachine pc)
    {
        HUDManager.instance.QueueDialogue(failureMessage);
        pc.cb.GlobalPosition = playerWarpPosition.GlobalPosition;
        pc.cb.GlobalRotation = playerWarpPosition.GlobalRotation;
    }

    public override bool ManageInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Interact"))
        {
            return !HUDManager.instance.TryAdvanceDialogue();
        }
        return false;
    }
}
