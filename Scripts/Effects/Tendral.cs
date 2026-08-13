using Godot;
using System;

public partial class Tendral : Node3D
{
	[Export] protected float baseLength = 1.645f;
	[Export] protected Path3D path;
	[Export] protected Skeleton3D skeleton;
	[Export] protected Node3D marker;

	// Called when the node enters the scene tree for the first time.	float timer = 0;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		FixScale();
	}

	public virtual void FixScale()
	{
        var pos = skeleton.GlobalPosition;
        var dis = pos.DistanceTo(marker.GlobalPosition);
        var scale = dis / baseLength;
        scale = (float)Mathf.Clamp(scale, 1, 5f);
        //GD.Print(scale);
        skeleton.SetBonePoseScale(2, Vector3.One.ReplaceY(scale));
    }
}
