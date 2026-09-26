using Godot;
using System;

public partial class LungeAttack : IHE_Attack
{
	[Export] float lungeSpeed;
	[Export] float speedMod = 1;

	public override EnemyState PhysicsProcess(double delta)
	{
		enem.cb.Velocity = enem.meshRoot.Basis.Z * lungeSpeed * speedMod;
		enem.cb.MoveAndSlide();
		return null;

	}
}
