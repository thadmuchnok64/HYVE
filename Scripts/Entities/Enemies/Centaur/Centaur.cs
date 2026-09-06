using Godot;
using System;
using System.Diagnostics;

public partial class Centaur : Enemy
{
	 
	[Export] protected Mesh headlessMesh2;
	[Export] int firstDecapitationThreshhold = 100;
	bool decapitated = false;

	public void CheckForPrimaryDecapitation()
	{
		if (decapitated)
			return;
		if(health <= firstDecapitationThreshhold)
		{
			var inst = bloodSplat.Instantiate();
			cb.AddSibling(inst);
			((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;
			meshInstance.Mesh = headlessMesh2;
		}

	}

}
