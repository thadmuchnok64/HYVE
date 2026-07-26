using Godot;
using System;

public partial class Scrawny : Enemy
{

	[Export] Godot.Collections.Array<AudioStream> footsteps;
	[Export] Node3D neckPoint;
	[Export] protected PackedScene bloodSplatNeck;
    [Export] protected ChainParticleTrigger bloodMistNeck;
    [Export] protected PackedScene headNode;
	[Export] Node3D headPoint;




    public void Footstep()
	{
		SoundManager.Instance.RequesetSFXSoundAtLocation(footsteps.PickRandom(), GlobalPosition);
	}

    public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void NeckSprayBlood()
	{
		var inst = bloodSplatNeck.Instantiate();
		cb.AddSibling(inst);
		((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;
		((Node3D)inst).GlobalRotation = bloodPoint.GlobalRotation;

	}

	public void BloodMistNeck()
	{
		bloodMistNeck.Launch();
        var inst = headNode.Instantiate();
        cb.AddSibling(inst);
        ((Node3D)inst).GlobalPosition = headPoint.GlobalPosition;
        ((Node3D)inst).GlobalRotation = headPoint.GlobalRotation;
		((PhysicalParticleLauncher)inst).Launch();
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BRAIN, bloodPoint.GlobalPosition);
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BRAIN, bloodPoint.GlobalPosition);

	}
}
