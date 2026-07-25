using Godot;
using System;

public partial class IHE_Attack: EnemyState
{
	[Export] float attackDistance;
	[Export] EnemyState idleState;
	[Export] EnemyState recoilState;
	[Export] EnemyState postureBreakState;
	[Export] EnemyState deadState;
	[Export] IHE_Attack followUpAttack; 

	[Export] float cooldownBetweenAttack = -1f;
	[Export] float attackOutTime = .5f;
	float attackTimer;
	float cooldownTimer = 0;

	public virtual bool IsAttackValid()
	{
		return cooldownTimer <= 0 && GlobalPosition.DistanceTo(GameMaster.Instance.GetPlayer().GlobalPosition)<attackDistance;
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
		{
			if (followUpAttack != null && followUpAttack.IsAttackValid())
				return followUpAttack;
			else
				return idleState;
		}
		return base.Process(delta);
	}

	public override EnemyState Enter(Enemy enemy)
	{
		cooldownTimer = cooldownBetweenAttack;
		attackTimer = attackOutTime;
		base.Enter(enemy);
		enem.anim.Set("parameters/attack/Transition/transition_request", animMeta);
		return this;

	}


	public override EnemyState HitEvent()
{
		if (!enem.alive)
			return deadState;
	if(enem.posture <= 0)
	{
		return postureBreakState;
	}
	else
	{
		return recoilState;
	}
}
}
