using Godot;
using System;

public partial class WombSpawn : EnemyState
{

	[Export] PackedScene enemyToSpawn;
	[Export] Node3D spawnPoint;
	[Export] EnemyState idleState;
	[Export] EnemyState recoilState;
	[Export] Node3D spawnParent;
	[Export] float transitionTime = .3f;
	[Export] AudioStream spawnSFX;
	[Export] GpuParticles3D spawnParticles;
	float timer = 0;


	public override EnemyState Enter(Enemy enemy)
	{
		timer = 0;
		Spawn();
		return base.Enter(enemy);
	}

	public override EnemyState Process(double delta)
	{
		timer += (float)delta;
		if (timer > transitionTime)
			return idleState;
		return base.Process(delta);
	}

	private void Spawn()
	{
		SoundManager.Instance.RequesetSFXSoundAtLocation(spawnSFX, spawnPoint.GlobalPosition);
		var newEnemy = enemyToSpawn.Instantiate();
		spawnParticles.Emitting = true;
		spawnParent.AddChild(newEnemy);
		((Node3D)newEnemy).GlobalPosition = spawnPoint.GlobalPosition;

	}

	public override EnemyState HitEvent()
	{
		return recoilState;
	}
}
