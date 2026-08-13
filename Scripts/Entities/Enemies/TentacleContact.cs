using Godot;
using System;

public partial class TentacleContact : EnemyMeleeContact
{
    [Export] public Tentacle tentacle;

    public override void OnWeaponHit(Node3D body)
    {
        if (tentacle.CanAttack())
        {
            base.OnWeaponHit(body);
            tentacle.Retract();
        }
    }
}
