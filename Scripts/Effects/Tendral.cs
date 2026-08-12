using Godot;
using System;

public partial class Tendral : Node3D
{
	[Export] float baseLength = 1.645f;
	[Export] Path3D path;
	[Export] Skeleton3D skeleton;
	[Export] protected Node3D marker;

	// Called when the node enters the scene tree for the first time.	float timer = 0;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//marker.GlobalPosition = path.GlobalPosition + path.Curve.Sample(0, Mathf.Clamp(timer / timeToReach, 0, 1));

		var dis = skeleton.GlobalPosition.DistanceTo(marker.GlobalPosition);
		var scale = dis / baseLength;
		scale = (float)Mathf.Clamp(scale, 0.1, 5f);
		//GD.Print(scale);
		skeleton.SetBonePoseScale(0, Vector3.One.ReplaceY(scale));

	}
}
