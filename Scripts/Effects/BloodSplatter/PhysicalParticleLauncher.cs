using Godot;
using System;

public partial class PhysicalParticleLauncher : Node3D
{
	[Export] float force;
	[Export] float randomSpread = MathF.PI;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		foreach(RigidBody3D rb in GetChildren())
		{
			Vector3 direction = Basis.Z;
			Random rand = new Random();
			rb.LinearVelocity = rb.Basis.Z.AddSpreadToDirection(randomSpread) * force;

			//rb.ApplyCentralForce(
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
