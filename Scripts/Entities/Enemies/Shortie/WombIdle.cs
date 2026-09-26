using Godot;
using System;

public partial class WombIdle : EnemyState
{

	[Export] float spawnCooldown = 1.2f;
	[Export] float extraTimeOnChildCapacityReached = .8f;
	[Export] EnemyState spawnState;
	[Export] EnemyState recoilState;

	float spawnTimer = 0;

	public override EnemyState Enter(Enemy enemy)
	{
		spawnTimer = 0;
		return base.Enter(enemy);
	}

	public override EnemyState Process(double delta)
	{
		if (((ShortieWomb)enem).canProduceMoreChildren())
		{
			spawnTimer += (float)delta;
			if (spawnTimer > spawnCooldown)
				return spawnState;
		}
		else
		{
			spawnTimer = -extraTimeOnChildCapacityReached;
		}
			return base.Process(delta);
	}

	public override EnemyState HitEvent()
	{
		return recoilState;
	}
}
