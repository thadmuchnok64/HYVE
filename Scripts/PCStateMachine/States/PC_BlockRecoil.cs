using Godot;
using System;

public partial class PC_BlockRecoil : PCState
{

	[Export] PCState idleState;
	[Export] PCState blockState;
	[Export] float timeToTransition = .4f;
	[Export] AudioStream blockSFX;
	float timer;

	bool crouching = false;
	bool isBlocking = true;

	public override PCState Enter()
	{
		SoundManager.Instance.RequesetSFXSoundAtLocation(blockSFX, GlobalPosition);
		isBlocking = true;
		timer = 0;
		anim.Set($"parameters/{animMetaState}/{animMeta}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
		base.Enter();
		stateMachine.Slide();
		return null;
	}
	// Called when the node enters the scene tree for the first time.
	public override PCState ManageInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Attack"))
		{

		}
		if (@event.IsActionReleased("Block"))
		{
			isBlocking = false;
		}
		if (@event.IsActionPressed("Block"))
		{
			isBlocking = true;
		}
		return null;
	}
	public override PCState PhysicsProcess(double delta)
	{
		Vector2 movement = new Vector2(-meshRoot.GlobalBasis.Z.X, -meshRoot.GlobalBasis.Z.Z);// meshRoot.Basis.Z.X);
		_Move(movement, delta);
		base._PhysicsProcess(delta);
		cb.MoveAndSlide();
		return null;
	}

	public virtual void _Move(Vector2 forw, double delta)
	{
		var force = forw.Normalized() * moveSpeed;
		var addedForce = new Vector3(force.X, 0, force.Y);
		var dot = addedForce.Normalized().Dot((cb.Velocity * new Vector3(1, 0, 1)).Normalized());
		if (dot > 0)
		{
			var lerpVel = (cb.Velocity * new Vector3(1, 0, 1)).Lerp(addedForce, (float)delta * 40 * dot);
			cb.Velocity = (new Vector3(lerpVel.X, cb.Velocity.Y, lerpVel.Z));
		}
		else
		{
			cb.Velocity = (new Vector3(addedForce.X, cb.Velocity.Y, addedForce.Z));
			//GD.Print("trigger backflip");

		}
		//anim.Run();
		//storedHForc 
	}

	public override PCState Process(double delta)
	{
		timer += (float)delta;
		if (timer > timeToTransition)
		{
            anim.Set($"parameters/{animMetaState}/{animMeta}/request", (int)AnimationNodeOneShot.OneShotRequest.FadeOut);
            if (isBlocking)
				return blockState;
			else
				return idleState;
		}
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
		return this;
	}
}
