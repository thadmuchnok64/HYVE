using Godot;
using System;

public partial class PressableOrgan : InteractableObject
{
	[Export] Node3D playerWarpPosition;
	[Export] AnimationPlayer anim;
	[Export] float timeToFreePlayer = 2f;
	[Export] Node3D arrow;
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
		Pressurize();
		
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
		arrow.RotationDegrees = new Vector3(0, Mathf.Lerp(90,-90,MuseumManager.instance.airPressure), 0);
	}


	public async void Pressurize()
	{
		await ToSignal(GetTree().CreateTimer(.5f), SceneTreeTimer.SignalName.Timeout);
		MuseumManager.instance.LungPressure();

	}
	public override bool ManageInput(InputEvent @event)
	{

		return false;
	}
}
