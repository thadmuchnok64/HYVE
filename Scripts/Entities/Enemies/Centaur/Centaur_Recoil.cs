using Godot;
using System;

public partial class Centaur_Recoil : IHE_Recoil
{
	public override EnemyState Enter(Enemy enemy)
	{
		if (enem is Centaur)
		{
			((Centaur)enem).CheckForPrimaryDecapitation();
		}
		return base.Enter(enemy);
	}
}
