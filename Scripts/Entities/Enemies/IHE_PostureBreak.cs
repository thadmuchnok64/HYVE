using Godot;
using System;

public partial class IHE_PostureBreak : EnemyState
{
	[Export] float recoilTime = 3f;
	[Export] EnemyState movingState;
	//[Export] EnemyState recoilState;

	float timer = 0;

	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		timer = 0;
		enem.nav.TargetPosition = enem.player.Position;
		enem.anim.Set($"parameters/{animMetaState}/fall/request",(int)AnimationNodeOneShot.OneShotRequest.Fire);

		return null;
	}
	public override EnemyState Process(double delta)
	{
		timer += (float)delta;
		if (timer>recoilTime)
		{
			return movingState;
		}
		return null;
	}

	public override EnemyState HitEvent()
	{
		enem.anim.Set($"parameters/{animMetaState}/hit/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
        timer = 0;

        // if dead, return deadstate
        return null;
		//return recoilState
	}
}
