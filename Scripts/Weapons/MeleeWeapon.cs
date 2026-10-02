using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MeleeWeapon : Weapon
{
	[Export] AudioStream swingSFX;
    [Export] Godot.Collections.Array<AudioStream> impactSFX;
    [Export] AudioStream killingSFX;
	[Export] AudioStream criticalSFX;

	[Export] GpuParticles3D blockSparks;
	[Export] float heavyDamage = 50;
    [Export] float heavyPosture = 120;
	List<Enemy> enemiesHitThisSwing;
	[Export] Godot.Collections.Array<WeaponHitCollider> colliders;

	private AttackType currentHitboxToScan = AttackType.LIGHT;




	AttackType currentAttack = AttackType.LIGHT;
	/*
	public override void OnWeaponHit(Node3D body)
	{
		base.OnWeaponHit(body);
	}
	*/

	public override void _Ready()
	{
		enemiesHitThisSwing = new List<Enemy>();
	}

	public override void _Process(double delta)
	{
		if (active)
		{
			CheckSwingCollisions();
		}
	}


	public override void SetCurrentHitbox(AttackType type)
	{
		currentHitboxToScan = type;
	}
	public override void SetWeaponActive(bool _active)
	{
		base.SetWeaponActive(_active);
		enemiesHitThisSwing.Clear();
		if (_active)
		{
			aud.Stream = swingSFX;
			aud.Play();
			CheckSwingCollisions();
		}
	}

	public override void FirstHitEvent()
	{
		base.FirstHitEvent();
		aud.Stream = impactSFX.PickRandom();
		aud.Play();
	}

	public override void KillingBlow()
	{
		base.KillingBlow();
		aud.Stream = killingSFX;
		aud.Play();
		TimeManager.instance.Hitstop();

	}

	public override void Block()
	{
		blockSparks.Emitting = true;
	}


    public override float NetDamage()
    {
		switch (currentAttack) {
			case AttackType.LIGHT:
				return base.NetDamage();
			//case AttackType.HEAVY:

					//return heavyDamage;

		}
        return base.NetDamage();

    }

    public override float NetPosture()
    {
        switch (currentAttack)
        {
            case AttackType.LIGHT:
                return base.NetDamage();
            //case AttackType.HEAVY:

                //return heavyPosture;

        }
        return base.NetDamage();

    }

	public void CheckSwingCollisions()
	{
		List<Node3D> overlapping = GetNewOverlappingBodies();

		if (overlapping == null)
			return;
		foreach (Node3D bod in overlapping)
		{
			foreach (Node em in bod.GetChildren())
			{
				if (em is Enemy && ((Enemy)em).alive)
				{
					if (!enemiesHitThisSwing.Contains(em))
						((Enemy)em).HitEnemyFromDirection(NetDamage(), NetPosture(), swingDir);
					if (((Enemy)em).alive)
					{
						if (enemiesHitThisSwing.Count <= 0)
							FirstHitEvent();
					}
					else
					{
						KillingBlow();
					}
					enemiesHitThisSwing.Add(((Enemy)em));
					OnWeaponHit((Enemy)em);
				}
			}
		}
	}

	public List<Node3D> GetNewOverlappingBodies()
	{
		var validCols = colliders.Where(c => c.attackType == currentHitboxToScan);
		if (validCols.Any())
		{
			var col = colliders.Where(c => c.attackType == currentHitboxToScan).First();
			var bodiesDetectedThisFrame = col.GetOverlappingBodies();
			return bodiesDetectedThisFrame.ToList();
		}
		else
			return null;
	}

}
