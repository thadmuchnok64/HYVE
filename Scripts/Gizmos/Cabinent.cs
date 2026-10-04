using Godot;
using System;

public partial class Cabinent : InteractableObject
{
	[Export] Node3D camPivot;
	[Export] AnimationPlayer anim;
	public override void TriggerGizmo(PCStateMachine pc)
	{
		((CameraController)GameMaster.Instance.mainCamRef).SetTrackingObject(camPivot);
		anim.Play(animationMeta);
	}
}
