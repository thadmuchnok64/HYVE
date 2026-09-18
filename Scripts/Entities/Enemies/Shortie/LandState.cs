using Godot;
using System;

public partial class LandState : EnemyState
{
	[Export] float timeToTransition = .5f;

	public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		enem.meshRoot.RotationDegrees = new Vector3(-90, 0, 0);//reset to default orientation;
		enem.cb.Velocity = Vector3.Zero;
		return null;
	}
}
