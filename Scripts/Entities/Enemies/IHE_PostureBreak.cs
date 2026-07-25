using Godot;
using System;

public partial class IHE_PostureBreak : EnemyState
{
	[Export] float recoilTime = 3f;
    [Export] float timeToStartRise = 2f;
	bool rising = false;
    [Export] EnemyState movingState;
	[Export] EnemyState deadState;
	//[Export] EnemyState recoilState;

	float timer = 0;

	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		timer = 0;
		enem.nav.TargetPosition = GameMaster.Instance.GetPlayer().Position;
		enem.anim.Set($"parameters/{animMetaState}/fall/request",(int)AnimationNodeOneShot.OneShotRequest.Fire);
        enem.anim.Set("parameters/prone/rise/request", (int)AnimationNodeOneShot.OneShotRequest.Abort);
		rising = false;
        return null;
	}
	public override EnemyState Process(double delta)
	{
		timer += (float)delta;
		if (enem.alive && timer>recoilTime)
		{
			return movingState;
		}
		if(!rising && timer>= timeToStartRise)
		{
			Rise();
		}
		return null;
	}

	public override EnemyState HitEvent()
	{
		enem.anim.Set($"parameters/{animMetaState}/hit/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
        enem.anim.Set("parameters/prone/rise/request", (int)AnimationNodeOneShot.OneShotRequest.Abort);
		rising = false;
        timer = 0;
		if (!enem.alive)
			return deadState;
		// if dead, return deadstate
		return null;
		//return recoilState
	}

	public void Rise()
	{
		rising = true;
		enem.anim.Set("parameters/prone/rise/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
	}
}
