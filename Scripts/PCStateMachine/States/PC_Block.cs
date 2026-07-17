using Godot;
using System;

public partial class PC_Block : PCState
{

	[Export] PCState walkState;
	[Export] PCState idleState;
	[Export] PCState recoilState;

	[Export] float dragForce;

	[Export] string animWalkBlendMeta;

	bool crouching = false;
	// Called when the node enters the scene tree for the first time.
	public override PCState ManageInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Attack"))
		{

		}
		if (@event.IsActionReleased("Block"))
		{
			return idleState;
		}
		return null;
	}
	public override PCState PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);


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

		var hVel = new Vector3(cb.Velocity.X, 0, cb.Velocity.Z);
		if (stateMachine.tracking)
		{
			meshRoot.LookAt(stateMachine.trackingObject.GlobalPosition, Vector3.Up);
			meshRoot.Rotation = new Vector3(0, meshRoot.Rotation.Y + MathF.PI, 0);
			var vec = new Vector2(meshRoot.Basis.X.Dot(cb.Velocity), meshRoot.Basis.Z.Dot(cb.Velocity)).Normalized();
			GD.Print(vec);
			anim.Set(animWalkBlendMeta, vec);
		}
		else if (hVel.Length() > .2f)
		{
			anim.Set(animWalkBlendMeta, new Vector2(0, 1));
			meshRoot.LookAt(cb.Position - hVel.Normalized() * 5, Vector3.Up);
			meshRoot.Rotation = new Vector3(0, meshRoot.Rotation.Y, 0);
		}
		//anim.Set(animMeta, Mathf.Clamp(hVel.Length() / animLerpMod, 0, 1));


		cb.MoveAndSlide();
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

	public override PCState Process(double delta)
	{
		crouching = Input.IsActionPressed("Crouch");
		if (crouching)
		{
			anim.Set($"parameters/{animMetaState}/Transition/transition_request", "crouch");
		}
		else
		{
			anim.Set($"parameters/{animMetaState}/Transition/transition_request", "stand");

		}
		return base.Process(delta);
	}

	public override PCState HitByEnemyEvent()
	{
		return recoilState;
	}
}
