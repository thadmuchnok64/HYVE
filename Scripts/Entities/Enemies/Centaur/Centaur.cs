using Godot;
using System;
using System.Diagnostics;

public partial class Centaur : Enemy
{

	public override void SwitchState(EnemyState state)
	{
		base.SwitchState(state);
		GD.Print(currentState.Name);
	}
}
