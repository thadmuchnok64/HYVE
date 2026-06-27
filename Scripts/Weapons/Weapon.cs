using Godot;
using System;
using System.Collections.Generic;

public enum SwingDirection { RIGHT, LEFT, UP }
public partial class Weapon : Node3D
{
	[Export] float baseDamage = 20;
	protected SwingDirection swingDir;
	List<Enemy> hitEnemiesThisSwing;
	bool active = false;

	public override void _Ready()
	{
		base._Ready();
		hitEnemiesThisSwing = new List<Enemy>();
	}

	public void SetWeaponActive(bool _active)
	{
		active = _active;
		if (active)
			hitEnemiesThisSwing.Clear();

	}
	public virtual void OnWeaponHit(Node3D body)
	{
		if (!active)
			return;
		foreach(Node em in body.GetChildren())
		{

			if (em is Enemy)
			{
				if(!hitEnemiesThisSwing.Contains((Enemy)em))
				(em as Enemy).HitEnemyFromDirection(baseDamage, swingDir);
				hitEnemiesThisSwing.Add((Enemy)em);

			}
		}
	}
}
