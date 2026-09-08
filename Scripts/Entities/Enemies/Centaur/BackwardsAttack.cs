using Godot;
using System;

public partial class BackwardsAttack : IHE_Attack
{
	[Export] float dotCutoff = -.2f;
	public override bool IsAttackValid(Enemy enemy)
	{
		enem = enemy;
		if (enem.meshRoot.GlobalBasis.Z.Dot(GameMaster.Instance.GetPlayer().GlobalPosition - enem.meshRoot.GlobalPosition) < -dotCutoff)
		{
			GD.Print("pass");
			return base.IsAttackValid(enem);
		}
		else
			return false;
	}
}
