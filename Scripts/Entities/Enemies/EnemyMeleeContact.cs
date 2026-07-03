using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyMeleeContact : Area3D
{
	[Export] float baseDamage = 25f;
	[Export] AudioStream attackHitSFX;

	List<PCStateMachine> hitEnemiesThisSwing;

	bool active = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hitEnemiesThisSwing = new List<PCStateMachine>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void SetWeaponActive(bool _active)
	{
		active = _active;
		Monitoring = active;
		if (active)
			hitEnemiesThisSwing.Clear();

	}

	public virtual void OnWeaponHit(Node3D body)
	{
		GD.Print(active);

		if (!active)
			return;
		foreach (Node3D pc in body.GetChildren())
		{

			if (pc is PCStateMachine)
			{
				//if (((PCStateMachine)pc).alive)
				//{
					if (!hitEnemiesThisSwing.Contains((PCStateMachine)pc))
						(pc as PCStateMachine).HitByEnemy(baseDamage);
					if (((PCStateMachine)pc).alive)
					{
						if (hitEnemiesThisSwing.Count <= 0)
							FirstHitEvent(pc);
					}
					else
					{
						KillingBlow();
					}
					hitEnemiesThisSwing.Add((PCStateMachine)pc);
				//}

			}
		}
	}

	public virtual void KillingBlow()
	{
		//base.KillingBlow();
		//aud.Stream = killingSFX;
		//aud.Play();
	}

	public virtual void FirstHitEvent(Node3D pc)
	{
		GD.Print(pc.GlobalPosition);
		SoundManager.Instance.RequesetSFXSoundAtLocation(attackHitSFX,pc.GlobalPosition);
		//base.FirstHitEvent();
		//aud.Stream = impactSFX;
		//aud.Play();
	}

}
