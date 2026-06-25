using Godot;
using System;

public partial class PC_Idle : PCState
{

	[Export] PCState walkState;
	[Export] PCState attackState;

	[Export] float dragForce;
	// Called when the node enters the scene tree for the first time.
	public override PCState ManageInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Attack"))
		{
			return attackState;
		}
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
		Vector2 movement = new Vector2(Input.GetAxis("MoveLeft", "MoveRight"), Input.GetAxis("MoveUp", "MoveDown"));
		if (movement.Length() > .1f)
		{
			return walkState;
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
			cb.LookAt(cb.Position - hVel.Normalized() * 5, Vector3.Up);
			cb.Rotation = new Vector3(0, cb.Rotation.Y, 0);
		}
		if (cb.IsOnFloor())
		{
			//if (Input.GetActionStrength("Sprint") > .1)
			//{
				//return sprintState;
			//}

		}

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

}
