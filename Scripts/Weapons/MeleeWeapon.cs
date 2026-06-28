using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeWeapon : Weapon
{
	[Export] AudioStream swingSFX;
	[Export] AudioStream impactSFX;
	[Export] AudioStream killingSFX;
	public override void OnWeaponHit(Node3D body)
	{
		base.OnWeaponHit(body);
	}

	public override void SetWeaponActive(bool _active)
	{
		base.SetWeaponActive(_active);
		if (_active)
		{
			aud.Stream = swingSFX;
			aud.Play();
		}
	}

	public override void FirstHitEvent()
	{
		base.FirstHitEvent();
		aud.Stream = impactSFX;
		aud.Play();
	}

	public override void KillingBlow()
	{
		base.KillingBlow();
		aud.Stream = killingSFX;
		aud.Play();
	}
}
