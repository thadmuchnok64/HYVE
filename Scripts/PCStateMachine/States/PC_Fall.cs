using Godot;
using System;

public partial class PC_Fall : PCState
{

	[Export] PCState idleState;
	[Export] PCState walkState;
	[Export] PCState sprintState;

	[Export] float maxFallVel = 5f;
	[Export] Curve velocityFalloff;
	[Export] float timeToMax = 1.5f;

	Vector3 startingVelocity;
	float timer = 0;
	public override PCState Enter()
	{
		timer = 0;
		startingVelocity = cb.Velocity;
		anim.Set($"parameters/conditions/landing", false);
		return base.Enter();
	}

	public override PCState Process(double delta)
	{
		timer += (float)delta;
		return base.Process(delta);
	}

	public override PCState PhysicsProcess(double delta)
	{
		if (cb.IsOnFloor())
		{
			cb.Velocity = cb.Velocity.ReplaceY(0);
			cb.MoveAndSlide();
			anim.Set($"parameters/conditions/landing", true);

			Vector2 movement = new Vector2(Input.GetAxis("MoveLeft", "MoveRight"), Input.GetAxis("MoveUp", "MoveDown"));
			if (movement.Length() > .1f)
			{
				if (!Input.IsActionPressed("Sprint"))
					return walkState;
				else
					return sprintState;
			}
			else
			{
				return idleState;
			}
		}
		else
		{
			cb.Velocity = startingVelocity.ReplaceY(0).Lerp(Vector3.Zero.ReplaceY(-maxFallVel), velocityFalloff.Sample((timer/timeToMax).Clamp01()));
		}
		cb.MoveAndSlide();
		return base.PhysicsProcess(delta);
	}
}
