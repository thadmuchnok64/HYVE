using Godot;
using System;

public partial class TentacleProjectile : Projectile
{
	public Tentacle tentacle;
	Vector3 posToTransitionTo;
	bool integrate = false;
	public override void HitEvent(Node hit)
	{
		tentacle.HitEvent(this);
		base.HitEvent(hit);
		Freeze = true;
	}

	public void SetPos(Vector3 newPos)
	{
		integrate = true;
		posToTransitionTo = newPos;
	}

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		base._IntegrateForces(state);
		if (integrate)
		{
			integrate = false;
		}
	}
}
