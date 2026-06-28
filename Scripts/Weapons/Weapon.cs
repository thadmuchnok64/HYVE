using Godot;
using System;
using System.Collections.Generic;

public enum SwingDirection { RIGHT, LEFT, UP }
public partial class Weapon : Node3D
{
	[Export] float baseDamage = 20;
	[Export] protected AudioStreamPlayer3D aud;

	protected SwingDirection swingDir;
	List<Enemy> hitEnemiesThisSwing;
	bool active = false;

	public override void _Ready()
	{
		base._Ready();
		hitEnemiesThisSwing = new List<Enemy>();
	}

	public virtual void SetWeaponActive(bool _active)
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
				if (((Enemy)em).alive)
				{
					if (!hitEnemiesThisSwing.Contains((Enemy)em))
						(em as Enemy).HitEnemyFromDirection(baseDamage, swingDir);
					if (((Enemy)em).alive)
					{
						if (hitEnemiesThisSwing.Count <= 0)
							FirstHitEvent();
					}
					else
					{
						KillingBlow();
					}
						hitEnemiesThisSwing.Add((Enemy)em);
				}

			}
		}
	}

	public virtual void FirstHitEvent()
	{

	}

	public virtual void KillingBlow()
	{

	}
}
