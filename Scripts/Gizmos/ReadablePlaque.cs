using Godot;
using System;

public partial class ReadablePlaque : InteractableObject
{

	public override void TriggerGizmo(PCStateMachine pc)
	{
		base.TriggerGizmo(pc);
		HUDManager.instance.QueueDialogue(defaultMessage);
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
