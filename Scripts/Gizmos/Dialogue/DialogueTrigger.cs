using Godot;
using System;

public partial class DialogueTrigger : InteractableObject
{
	[Export] Node3D camPivot;
	PCStateMachine pc;
	[Export] AudioStreamPlayer3D aud;
	[Export] DA_DialogueTree dialogue;
	[Export] Node3D dialoguePivot;
	public override void TriggerGizmo(PCStateMachine pc)
	{
		base.TriggerGizmo(pc);
		this.pc = pc;
		((CameraController)GameMaster.Instance.mainCamRef).SetTrackingObject(camPivot);
		HUDManager.instance.RequestDialogue(dialogue, dialoguePivot);
	}

	public override bool ManageInput(InputEvent @event)
	{
		/*
		if (@event.IsActionPressed("Interact"))
		{
			pc.SetDefaultCamPoint();
			return true;
		}
		*/
		return false;
	}
}
