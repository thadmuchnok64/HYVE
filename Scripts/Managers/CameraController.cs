using Godot;
using System;

public partial class CameraController : Camera3D
{

	[Export] Node3D currentTrackingObject;
	[Export] Curve switchPositionCurve;
	[Export] float timeToSwitchPos = .3f;
	float timer = 5;
	Vector3 prevPosition = Vector3.Zero;
	Vector3 prevRot;
	public void SetTrackingObject(Node3D newObj) // might change this later to use camera states instead.
	{
		prevPosition = GlobalPosition;
		prevRot = GlobalRotation;
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
		GlobalRotation = prevRot.Lerp(currentTrackingObject.GlobalRotation, val);
		//Quaternion = prevRot.Slerp(currentTrackingObject.Basis.GetRotationQuaternion(), val);
		base._Process(delta);
    }
}
