using Godot;
using System;

public partial class CameraController : Camera3D
{

	[Export] Node3D currentTrackingObject;
	[Export] Curve switchPositionCurve;
	[Export] float timeToSwitchPos = .3f;
	[Export] Curve cameraCurve;
	[Export] Node3D mouseRay;
	float timer = 5;
	Vector3 prevPosition = Vector3.Zero;
	Quaternion prevRot;
	public void SetTrackingObject(Node3D newObj) // might change this later to use camera states instead.
	{
		prevPosition = GlobalPosition;
		prevRot = GlobalBasis.GetRotationQuaternion();
		currentTrackingObject = newObj;
		timer = 0;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += (float)delta;
		var val = switchPositionCurve.Sample(Math.Clamp(timer / timeToSwitchPos, 0, 1));

        GlobalPosition = prevPosition.Lerp(currentTrackingObject.GlobalPosition, val);
		GlobalRotation = prevRot.Slerp(currentTrackingObject.GlobalBasis.GetRotationQuaternion(),cameraCurve.Sample(val)).GetEuler();
		//Quaternion = prevRot.Slerp(currentTrackingObject.Basis.GetRotationQuaternion(), val);
		mouseRay.GlobalPosition = ProjectPosition(GetViewport().GetMousePosition(),.1f);
		mouseRay.LookAt(ProjectPosition(GetViewport().GetMousePosition(), 1f));
		base._Process(delta);
    }
}
