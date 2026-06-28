using Godot;
using System;

public partial class IHE_Death : EnemyState
{
	//[Export] EnemyState recoilState;


	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		//enem.nav.TargetPosition = enem.player.Position;
		//enem.anim.Set($"parameters/{animMetaState}/fall/request",(int)AnimationNodeOneShot.OneShotRequest.Fire);

		return null;
	}
	public override EnemyState Process(double delta)
	{
		return null;
	}

	public override EnemyState HitEvent()
	{
		//enem.anim.Set($"parameters/{animMetaState}/hit/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);

        // if dead, return deadstate
        return null;
		//return recoilState
	}
}
