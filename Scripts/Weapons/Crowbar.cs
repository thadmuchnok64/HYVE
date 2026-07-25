using Godot;
using System;

public partial class Crowbar : MeleeWeapon
{
    [Export] ChainParticleTrigger impactParticles;
    public override void OnWeaponHit(Node3D body)
    {
        base.OnWeaponHit(body);
        impactParticles.Launch();
    }
}
