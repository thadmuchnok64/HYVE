using Godot;
using System;

public partial class PCState : Node3D
{
	[Export] protected PCStateMachine stateMachine;
	Node3D camPoint;
	[Export] float moveSpeed = 50;
	[Export] protected string animMetaState;
	[Export] protected string animMeta;
	protected Node3D meshRoot;



	protected CharacterBody3D cb;
	protected AnimationTree anim;
	public virtual PCState Enter()
	{
		return null;
		//do animation here
	}

	public virtual PCState Exit()
	{
		return null;
	}

	public virtual void ExitAnimation()
	{
		anim.Set($"parameters/conditions/{animMetaState}", false);
		//anim.Set($"parameters/conditions/{animationName}", false);
	}

	public virtual void EntryAnimation()
	{
		anim.Set($"parameters/conditions/{animMetaState}", true);

		//anim.Set($"parameters/conditions/{animationName}", true);

	}

	public virtual PCState ManageInput(InputEvent @event)
	{
		return null;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cb = stateMachine.cb;
		camPoint = stateMachine.camPoint;
		anim = stateMachine.anim;
		meshRoot = stateMachine.meshRoot;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public virtual PCState Process(double delta)
	{

		return null;
	}

	public virtual PCState PhysicsProcess(double delta)
	{
		return null;
	}


	public virtual void _Move(Vector2 forw, double delta)
	{
		var force = forw.Normalized() * moveSpeed;
		var addedForce = new Vector3(force.X, 0, force.Y);
		addedForce = addedForce.Rotated(new Vector3(0, 1, 0), camPoint.GlobalRotation.Y);
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
}
