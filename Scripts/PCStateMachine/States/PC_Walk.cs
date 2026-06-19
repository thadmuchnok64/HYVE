using Godot;
using System;

public partial class PC_Walk : PCState
{

	[Export] PCState idleState;

	[Export] float minSpeed = .05f;
	[Export] float maxSpeed = 5f;

	[Export] float dragForce;
	// Called when the node enters the scene tree for the first time.
	public override PCState ManageInput(InputEvent @event)
	{
		/*
		if (@event.IsActionPressed("Jump"))
		{
			if (cb.IsOnFloor())
				return jumpState;
		}
		*/
		return null;
	}
	public override PCState PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		/*
		if (cb.Velocity.Y < 0.2f && !cb.IsOnFloor())
		{
			return fallState;
		}
		*/
		//movement
		Vector2 movement = new Vector2(Input.GetAxis("MoveRight", "MoveLeft"), Input.GetAxis("MoveDown", "MoveUp"));
		if (movement.Length() > .1f)
		{
			_Move(movement, delta);
		}
		else
		{
			_SlowGroundMovement(delta);
		}
		//Gravity
		//_ApplyGravity(delta);

		cb.MoveAndSlide();


		var hVel = new Vector3(cb.Velocity.X, 0, cb.Velocity.Z);
		if (hVel.Length() > .2f)
		{
			//cb.LookAt(cb.Position - hVel.Normalized() * 5, Vector3.Up);
			//cb.Rotation = new Vector3(0, cb.Rotation.Y, 0);
		}
		if (cb.IsOnFloor())
		{
			if (Input.GetActionStrength("Sprint") > .1)
			{
				//return sprintState;
			}
			if (cb.Velocity.Length() > 0.05f)
			{
				return null;
			}
			else
			{
				return idleState;
			}
		}

		return null;

	}

	public override PCState Process(double delta)
	{

		var timeScale = Mathf.Clamp((cb.Velocity.Length() - minSpeed) / (maxSpeed - minSpeed), 0, 1);
		//anim.Set("parameters/Ground/RunTimeScale/scale", timeScale);

		return null;
	}

	private void _SlowGroundMovement(double delta)
	{
		var newLen = (cb.Velocity.Length() - dragForce * (float)delta);
		if (newLen <= 0)
		{
			cb.Velocity = new Vector3(0, cb.Velocity.Y, 0);
			return;
		}
		cb.Velocity = cb.Velocity.Normalized() * newLen;
	}

	public override void EntryAnimation()
	{
		//anim.Set("parameters/Ground/GroundBlend/transition_request", animationName);
	}

	public override void ExitAnimation()
	{
		//base.ExitAnimation();
	}
}
