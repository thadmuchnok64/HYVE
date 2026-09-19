using Godot;
using System;

public partial class LandState : EnemyState
{
	[Export] float timeToTransition = .5f;
	[Export] GpuParticles3D launchingParticles;
	[Export] GpuParticles3D splatParticles;
	[Export] EnemyState runningState;
	[Export] Node3D meshPivot;

	[Export] AudioStream splatSFX;
	float timer = 0;

	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		timer = 0;
		SoundManager.Instance.RequesetSFXSoundAtLocation(splatSFX, GlobalPosition);
		launchingParticles.Emitting = false;
		splatParticles.Emitting = true;
		meshPivot.RotationDegrees = new Vector3(-90, 0, 0);//reset to default orientation;
		GoreManager.Instance.RequestBloodSplatAtLocation(GlobalPosition, BloodDecalType.MEDIUM);

		enem.cb.Velocity = Vector3.Zero;
		return null;
	}

	public override EnemyState Process(double delta)
	{
		timer += (float)delta;
		if (timer > timeToTransition)
			return runningState;
		return base.Process(delta);
	}
}
