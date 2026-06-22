using Godot;
using System;

public partial class EnemyState : Node
{
	protected Enemy enem;
	[Export] protected string animMeta;
	public virtual EnemyState Enter(Enemy enemy)
	{
		enem = enemy;
		return null;
		//do animation here
	}

	public virtual EnemyState Exit()
	{
		return null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public virtual EnemyState Process(double delta)
	{

		return null;
	}

	public virtual EnemyState PhysicsProcess(double delta)
	{
		return null;
	}

	public virtual EnemyState DetectPlayerEvent()
	{
		return null;
	}
}
