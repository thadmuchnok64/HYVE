using Godot;
using System;

public partial class PC_ForwardAttack : PC_Attack
{
	public override void _Move(Vector2 forw, double delta)
	{
		var force = forw.Normalized() * moveSpeed;
		var addedForce = new Vector3(force.X, 0, force.Y);
		addedForce = addedForce.Rotated(new Vector3(0, 1, 0), meshRoot.GlobalRotation.Y);
		var dot = addedForce.Normalized().Dot((cb.Velocity * new Vector3(1, 0, 1)).Normalized());
		if (dot > 0)
		{
			var lerpVel = (cb.Velocity * new Vector3(1, 0, 1)).Lerp(addedForce, (float)delta * 40 * dot);
			cb.Velocity = (new Vector3(lerpVel.X, cb.Velocity.Y, lerpVel.Z));
		}
		else
		{
			cb.Velocity = (new Vector3(addedForce.X, cb.Velocity.Y, addedForce.Z));
		}
	}
}
