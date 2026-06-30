using Godot;
using System;

public partial class PC_Attack : PCState
{

	[Export] PCState walkState;
	[Export] PCState idleState;
	[Export] PCState attackFollowup;
	[Export] float followUpMinTime = -1; // -1 = cant followup
	[Export] float tempLength = .8f;
	[Export] float attackStaminaCost = 25f;
	[Export] string animMeta2;

	bool crouching = false;
	float timer;

	[Export] float dragForce;
	// Called when the node enters the scene tree for the first time.
	public override PCState ManageInput(InputEvent @event)
	{
		if(followUpMinTime>-1 && timer >= followUpMinTime)
		if (@event.IsActionPressed("Attack"))
		{
				if (attackFollowup == null || !stateMachine.ConsumeStamina(attackStaminaCost))
					return null;
				if (attackFollowup == this)
					Enter();
				else
				return attackFollowup;
		}
		return null;
	}
	public override PCState PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		return null;

	}

	public override PCState Process(double delta)
	{
		timer += (float)delta;
		if (timer > tempLength)
			return idleState;
		return base.Process(delta);
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

	public override PCState Enter()
	{
		timer = 0;
		anim.Set($"parameters/{animMetaState}/Transition/transition_request", animMeta);
		if (animMeta2 != null)
		{
			anim.Set($"parameters/{animMetaState}/{animMeta2}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
		}
		cb.Velocity = Vector3.Zero;
		return base.Enter();
	}


}
