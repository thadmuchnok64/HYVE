using Godot;
using System;

public partial class IHE_Recoil : EnemyState
{
	[Export] protected float recoilTime = .3f;
	[Export] protected EnemyState movingState;
    [Export] protected EnemyState deadState;

    protected float timer = 0;

	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		timer = 0;
		if(enem.nav != null)
		enem.nav.TargetPosition = GameMaster.Instance.GetPlayer().Position;
		enem.anim.Set($"parameters/{animMetaState}/{animMeta}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
		return null;
	}
	public override EnemyState Process(double delta)
	{
		timer += (float)delta;
		if (timer>recoilTime)
		{
			if(enem.alive)
				return movingState;
			else
				return deadState;
		}
		return null;
	}

	public override EnemyState HitEvent()
	{
		if (!enem.alive)
			return deadState;
		Enter(enem); // reset recoil
		return base.HitEvent();

	}
}
