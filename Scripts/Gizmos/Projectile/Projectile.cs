using Godot;
using System;

public partial class Projectile : RigidBody3D
{
	[Export] float startingVelocity = 5;
	public virtual void Launch(Vector3 direction)
	{
		Freeze = false;
		LinearVelocity = direction * startingVelocity;
	}

	public virtual void HitEvent(Node hit) { }


}
