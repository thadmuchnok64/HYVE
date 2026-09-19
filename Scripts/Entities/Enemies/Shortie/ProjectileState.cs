using Godot;
using System;

public partial class ProjectileState : EnemyState
{
	[Export] float defaultVelocity;
	[Export] float horizontalLaunchVel = 5;
	[Export] float verticalLaunchVel = 5;
	[Export] float gravity = 9.8f;
	[Export] EnemyState landingState;
	[Export] Node3D meshPivot;
	[Export] GpuParticles3D launchingParticles;

	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		launchingParticles.Emitting = true;
		var hVel = StaticHelpers.RandomVector2D() * horizontalLaunchVel;

		enem.cb.Velocity = new Vector3(hVel.X, verticalLaunchVel, hVel.Y);
		return null;
	}

	public override EnemyState Process(double delta)
	{
		meshPivot.LookAt(enem.cb.GlobalPosition + enem.cb.Velocity);
		return base.Process(delta);
	}

	public override EnemyState PhysicsProcess(double delta)
	{
		if (enem.cb.IsOnFloor())
			return landingState;
		enem.cb.Velocity -= Vector3.Up * gravity * ((float)delta);
		enem.cb.MoveAndSlide();
		return base.PhysicsProcess(delta);
	}
}
