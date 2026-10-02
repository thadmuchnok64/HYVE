using Godot;
using System;

public partial class DialogueTrigger : InteractableObject
{
	[Export] Node3D camPivot;
	PCStateMachine pc;
	[Export] NPCEntity entity;
	[Export] Node3D dialoguePivot;

	[Export] Node3D playerPivot;
	[Export] float timeToSnapToPlayerPos;
	float timer = 999;
	Vector3 currentPlayerPos;

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (pc != null && timer < timeToSnapToPlayerPos)
		{
			timer += (float)delta;
			pc.SnapPlayerToPosition(currentPlayerPos.Lerp(playerPivot.GlobalPosition,Math.Clamp(timer/timeToSnapToPlayerPos,0,1)));
			pc.MeshLookAt(dialoguePivot.GlobalPosition);

		}
	}
	public override void TriggerGizmo(PCStateMachine pc)
	{
		base.TriggerGizmo(pc);
		this.pc = pc;
		DA_DialogueTree nextDialogue = entity.getCurrentDialogue();
		if (nextDialogue != null)
		{
			currentPlayerPos = pc.cb.GlobalPosition;
			((CameraController)GameMaster.Instance.mainCamRef).SetTrackingObject(camPivot);
			HUDManager.instance.RequestDialogue(nextDialogue, dialoguePivot);
			timer = 0;
		}
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
