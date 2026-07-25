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
		Vector2 movement = new Vector2(-meshRoot.Basis.Z.Z, meshRoot.Basis.Z.X);
		_Move(movement, delta);
		base._PhysicsProcess(delta);
		cb.MoveAndSlide();
		return null;
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
