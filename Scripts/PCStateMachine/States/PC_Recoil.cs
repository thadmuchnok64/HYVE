using Godot;
using System;

public partial class PC_Recoil : PCState
{

	[Export] PCState idleState;
	[Export] float smallLength = .8f;
	[Export] float bigLength = 1f;
	[Export] float fallLength = 1.5f;

	[Export] float bigRecoilThreshold = 40f;
	[Export] string animMetaSmall;
	[Export] string animMetaLarge;
	[Export] string animMetaFall;

	float tempLength;


	float timer;

	[Export] float dragForce;
	// Called when the node enters the scene tree for the first time.

	public override PCState Process(double delta)
	{
		timer += (float)delta;
		if (timer > tempLength)
			return idleState;
		return base.Process(delta);
	}


	public override PCState Enter()
	{
		timer = 0;

		if (stateMachine.posture < 0)
		{
			anim.Set($"parameters/{animMetaState}/{animMetaFall}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
			anim.Set($"parameters/{animMetaState}/Transition/transition_request", animMetaFall);
			tempLength = fallLength;
		}
		else if (stateMachine.posture < bigRecoilThreshold)
		{
			anim.Set($"parameters/{animMetaState}/{animMetaLarge}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
			anim.Set($"parameters/{animMetaState}/Transition/transition_request", animMetaLarge);
			tempLength = bigLength;

		}
		else
		{
			anim.Set($"parameters/{animMetaState}/{animMetaSmall}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
			anim.Set($"parameters/{animMetaState}/Transition/transition_request", animMetaSmall);
			tempLength = smallLength;

		}

		cb.Velocity = Vector3.Zero;
		return base.Enter();
	}


}
