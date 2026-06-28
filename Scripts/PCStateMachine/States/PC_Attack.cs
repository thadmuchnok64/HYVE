using Godot;
using System;

public partial class PC_Attack : PCState
{

	[Export] PCState walkState;
	[Export] PCState idleState;
	[Export] float tempLength = .8f;
	bool crouching = false;
	float timer;

	[Export] float dragForce;
	// Called when the node enters the scene tree for the first time.
	public override PCState ManageInput(InputEvent @event)
	{
		//if (@event.IsActionPressed("Attack"))
	//	{
	//		return attackState;
	  //  }
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
		cb.Velocity = Vector3.Zero;
		return base.Enter();
	}


}
