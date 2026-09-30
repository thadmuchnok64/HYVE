using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public enum SwingDirection { RIGHT, LEFT, UP }
public enum AttackType { LIGHT, OVERHEAD, BLOCKFOLLOWUP,RUNNING}
public partial class Weapon : Node3D
{
	[Export] float baseDamage = 20;
	[Export] protected AudioStreamPlayer3D aud;


	protected SwingDirection swingDir;

	protected bool active = false;


	public virtual void SetCurrentHitbox(AttackType type)
	{
	}

	public virtual void SetWeaponActive(bool _active)
	{
		active = _active;
	}


	public virtual void OnWeaponHit(Node3D body)
	{

	}



	public virtual float NetDamage()
	{
		return baseDamage;
	}

    public virtual float NetPosture()
    {
        return baseDamage;
    }

	//DEPRECIATED
	/*
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
					if (!enemiesHitThisSwing.Contains((Enemy)em))
						(em as Enemy).HitEnemyFromDirection(NetDamage(),NetPosture(), swingDir);
					if (((Enemy)em).alive)
					{
						if (enemiesHitThisSwing.Count <= 0)
							FirstHitEvent();
					}
					else
					{
						KillingBlow();
					}
					enemiesHitThisSwing.Add((Enemy)em);
				}

			}
		}
	}
	
	public virtual void OnWeaponExit(Node3D body)
	{
		foreach (Node em in body.GetChildren())
		{

			if (em is Enemy)
			{
				if (((Enemy)em).alive)
				{
					if (enemiesHitThisSwing.Contains((Enemy)em))
					{
						enemiesHitThisSwing.Remove((Enemy)em);
					}
				}

			}
		}
	}

	

	void HitEnemy(Node3D body)
	{
		foreach (Node em in body.GetChildren())
		{

			if (em is Enemy)
			{
				if (((Enemy)em).alive)
				{
					if (!overlappingEnemies.Contains((Enemy)em))
					{
						(em as Enemy).HitEnemyFromDirection(NetDamage(), NetPosture(), swingDir);
						if (((Enemy)em).alive)
						{
							if (overlappingEnemies.Count <= 0)
								FirstHitEvent();
						}
						else
						{
							KillingBlow();
						}
						overlappingEnemies.Add((Enemy)em);
					}
				}

			}
		}
	}
	

	void OverlapEnemyOnly(Node3D body)
	{
		foreach (Node em in body.GetChildren())
		{

			if (em is Enemy)
			{
				if (((Enemy)em).alive)
				{
					if (!overlappingEnemies.Contains((Enemy)em))
					overlappingEnemies.Add((Enemy)em);
				}

			}
		}
	}
	*/

	public virtual void FirstHitEvent()
	{

	}

	public virtual void KillingBlow()
	{

	}

	public virtual void Block()
	{

	}
}
