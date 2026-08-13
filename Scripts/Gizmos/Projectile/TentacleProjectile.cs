using Godot;
using System;

public partial class TentacleProjectile : Projectile
{
	public Tentacle tentacle;
	Vector3 posToTransitionTo;
	bool integrate = false;
	[Export] GpuParticles3D particles;
	public override void HitEvent(Node hit)
	{
		tentacle.HitEvent(this);
		base.HitEvent(hit);
		Freeze = true;
		GoreManager.Instance.RequestBloodSplatAtLocation(GlobalPosition, BloodDecalType.SMALL);
		particles.Emitting = true;
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
