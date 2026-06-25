using Godot;
using System;

public partial class IHE_Idle: EnemyState
{
	[Export] float detectionDistance;
	[Export] EnemyState seekingState;


	public override EnemyState Enter(Enemy enemy)
	{
		return base.Enter(enemy);
	}

	public override EnemyState DetectPlayerEvent()
	{
		return seekingState;

	}
}
