using Godot;
using System;

public enum SwingDirection { RIGHT, LEFT, UP }
public partial class Weapon : Node3D
{
	[Export] float baseDamage = 20;
	protected SwingDirection swingDir;
	public virtual void OnWeaponHit(Node3D body)
	{
		foreach(Node em in body.GetChildren())
		{
			if(em is Enemy)
				(em as Enemy).HitEnemyFromDirection(baseDamage, swingDir);
		}
	}
}
