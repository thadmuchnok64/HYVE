using Godot;
using System;

public partial class Door : InteractableObject
{

    public override void TriggerGizmo(PCStateMachine pc)
    {
        HUDManager.instance.QueueDialogue(failureMessage);
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
