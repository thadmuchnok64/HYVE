using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeWeapon : Weapon
{
	[Export] AudioStream swingSFX;
    [Export] Godot.Collections.Array<AudioStream> impactSFX;
    [Export] AudioStream killingSFX;
	[Export] GpuParticles3D blockSparks;
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
		aud.Stream = impactSFX.PickRandom();
		aud.Play();
	}

	public override void KillingBlow()
	{
		base.KillingBlow();
		aud.Stream = killingSFX;
		aud.Play();
	}

	public override void Block()
	{
		blockSparks.Emitting = true;
	}
}
