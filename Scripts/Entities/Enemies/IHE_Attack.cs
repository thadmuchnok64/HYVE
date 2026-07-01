using Godot;
using System;

public partial class IHE_Attack: EnemyState
{
	[Export] float attackDistance;
	[Export] EnemyState idleState;
	

	[Export] float cooldownBetweenAttack = -1f;
	[Export] float attackOutTime = .5f;
	float attackTimer;
	float cooldownTimer = 0;

	public virtual bool IsAttackValid()
	{
		return cooldownTimer <= 0;
	}



	public override void _Process(double delta)
	{
		base._Process(delta);
		cooldownTimer -= (float)delta;
	}

	public override EnemyState Process(double delta)
	{
		attackTimer -= (float)delta;
		if (attackTimer <= 0)
			return idleState;
		return base.Process(delta);
	}

	public override EnemyState Enter(Enemy enemy)
	{
		cooldownTimer = cooldownBetweenAttack;
		attackTimer = attackOutTime;
		return base.Enter(enemy);
	}
	/*
public override EnemyState DetectPlayerEvent()
{
	return seekingState;

}

public override EnemyState HitEvent()
{
	if(enem.posture <= 0)
	{
		return postureBreakState;
	}
	else
	{
		return recoilState;
	}
}
*/
}
