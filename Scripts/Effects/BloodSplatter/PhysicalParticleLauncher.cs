using Godot;
using System;

public partial class PhysicalParticleLauncher : Node3D
{
	[Export] float force;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		foreach(RigidBody3D rb in GetChildren())
		{
			rb.LinearVelocity = StaticHelpers.RandomVector() * force;
			//rb.ApplyCentralForce(
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
