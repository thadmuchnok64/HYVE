using Godot;
using System;

public partial class Keypad : InteractableObject
{
	[Export] string key = "1234";
	[Export] Node3D camPivot;
	PCStateMachine pc;
	[Export] KeypadUI ui;
	[Export] AudioStreamPlayer3D aud;

	public bool inputLocked = false;

	public override void _Ready()
	{
		ui.key = key;
		ui.keypad = this;
	}

	public override void TriggerGizmo(PCStateMachine pc)
	{
		base.TriggerGizmo(pc);
		this.pc = pc;
		((CameraController)GameMaster.Instance.mainCamRef).SetTrackingObject(camPivot);
		((CameraController)GameMaster.Instance.mainCamRef).TriggerMouseInteraction(true);
	}

	public override bool ManageInput(InputEvent @event)
	{
		if (inputLocked)
			return false;
		if (@event.IsActionPressed("Interact"))
		{
			pc.SetDefaultCamPoint();
			((CameraController)GameMaster.Instance.mainCamRef).TriggerMouseInteraction(false);
			return true;
		}
		return false;
	}

	public void SendInt(int i)
	{
		if (inputLocked)
			return;

		if (ui.TypeNumber(i))
			Unlock();
	}
	public void ClearCode()
	{	
		ui.ResetCode();
	}

	public void Unlock() { }
}
