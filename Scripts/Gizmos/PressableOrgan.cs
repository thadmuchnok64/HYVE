using Godot;
using System;

public partial class PressableOrgan : InteractableObject
{
	[Export] Node3D playerWarpPosition;
	[Export] AnimationPlayer anim;
	[Export] float timeToFreePlayer = 2f;
	bool used = false;
	PCStateMachine pc;
	float activeTimer = 0f;

	public override void TriggerGizmo(PCStateMachine pc)
	{
		/*
		if (used)
		{
			used = false;
			pc.ForceUninteract();
			return;
		}
		else
		{
		*/
		activeTimer = 0;
			used = true;
			interactSuccess = true;
			//colliderToDisable.Disabled = true;
			//activeTimer = 0;
			//active = true;
			anim.Active = true;
			anim.Play("press");
			this.pc = pc;
			pc.cb.GlobalPosition = playerWarpPosition.GlobalPosition;
			pc.RotateMesh(playerWarpPosition.GlobalRotation);
		//}
	}
	public override void _Process(double delta)
	{
		if (used)
		{
			activeTimer += (float)delta;
			if (activeTimer > timeToFreePlayer)
			{
				pc.ForceUninteract();
				used = false;
			}
		}
	}

	public override bool ManageInput(InputEvent @event)
	{

		return false;
	}
}
