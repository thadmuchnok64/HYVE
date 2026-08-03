using Godot;
using System;

public partial class InteractableComputer : InteractableObject
{
	[Export] Node3D camPivot;
	[Export] BloxorGizmo bloxor; // change this. this script should probably be extended into BloxorComputer.cs or something
	PCStateMachine pc;
	[Export] AudioStreamPlayer3D aud;
	public override void TriggerGizmo(PCStateMachine pc)
	{
		base.TriggerGizmo(pc);
		this.pc = pc;
		bloxor.aud = aud;
		((CameraController)GameMaster.Instance.mainCamRef).SetTrackingObject(camPivot);
	}

	public override bool ManageInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact"))
		{
			pc.SetDefaultCamPoint();
			return true;
		}
		bloxor.ManageInput(@event);
		return false;
	}
}
