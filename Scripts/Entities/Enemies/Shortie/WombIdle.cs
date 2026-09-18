using Godot;
using System;

public partial class WombIdle : EnemyState
{

	[Export] float spawnCooldown = 1.2f;
	[Export] EnemyState spawnState;

	float spawnTimer = 0;

	public override EnemyState Enter(Enemy enemy)
	{
		spawnTimer = 0;
		return base.Enter(enemy);
	}

	public override EnemyState Process(double delta)
	{
		spawnTimer += (float)delta;
		if (spawnTimer > spawnCooldown)
			return spawnState;
		return base.Process(delta);
	}
}
