using Godot;
using System;

public partial class IHE_Recoil : EnemyState
{
	[Export] float recoilTime = .3f;
	[Export] EnemyState movingState;
	float timer = 0;

	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		timer = 0;
		enem.nav.TargetPosition = enem.player.Position;

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
}
