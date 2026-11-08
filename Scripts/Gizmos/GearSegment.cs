using Godot;
using System;

public partial class GearSegment : InteractableObject
{
	[Export] Node3D playerWarpPosition;
	[Export] float timeToFreePlayer = 2f;
	[Export] string itemNeededToProgress = "Gear";
	[Export] HamsterWheel hamsterWheel;

	PCStateMachine pc;

	float activeTimer = 0;
	bool active = false;
	bool used = false;
	public override void TriggerGizmo(PCStateMachine pc)
	{
		if (used)
		{
			pc.ForceUninteract();
			return;
		}
		if (pc.inventory.DoesHaveItem(itemNeededToProgress)) // check inventory for object here
		{
			used = true;
			interactSuccess = true;
			activeTimer = 0;
			active = true;
			this.pc = pc;
			pc.cb.GlobalPosition = playerWarpPosition.GlobalPosition;
			pc.RotateMesh(playerWarpPosition.GlobalRotation);
			hamsterWheel.InsertGear();
		}
		else
		{
			interactSuccess = false;
			HUDManager.instance.QueueDialogue(defaultMessage);
		}
	}

	public override bool ManageInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact"))
		{
			return !HUDManager.instance.TryAdvanceDialogue();
		}
		return false;
	}

	public override void _Process(double delta)
	{
		if (active)
		{
			activeTimer += (float)delta;
			if (activeTimer > timeToFreePlayer)
			{
				pc.ForceUninteract();
				active = false;
			}
		}
	}
}
