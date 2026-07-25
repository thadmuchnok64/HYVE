using Godot;
using System;

public partial class DecalEffect : Decal
{
	[Export] Curve spawnCurve;
	[Export] float timeToGrow = .4f;
	float timer = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += (float)delta;
		Scale = Vector3.One * spawnCurve.Sample(Mathf.Clamp(timer / timeToGrow, 0,1));
	}
}
