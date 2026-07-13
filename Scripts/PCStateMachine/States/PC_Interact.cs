using Godot;
using System;

public partial class PC_Interact : PCState
{

	[Export] PCState walkState;
	[Export] PCState idleState;
	[Export] float tempLength = .8f;
	InteractableObject interactable;

	float timer;

	[Export] float dragForce;
	// Called when the node enters the scene tree for the first time.

	public override PCState PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		return null;

	}

    public override PCState ManageInput(InputEvent @event)
    {
        if (interactable != null)
		{
			if (interactable.ManageInput(@event))
			{
				return idleState;
			}
			return null;
		}
		else
		{
			return idleState;
		}
    }



	public override PCState Enter()
	{
		timer = 0;
        interactable = stateMachine.TryInteraction();
		if (interactable == null)
			return idleState;
        anim.Set($"parameters/{animMetaState}/Transition/transition_request", interactable.animationMeta);
		/*
		if (animMeta2 != null)
		{
			anim.Set($"parameters/{animMetaState}/{animMeta2}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
		}
		*/
		cb.Velocity = Vector3.Zero;
		return base.Enter();
	}


}
