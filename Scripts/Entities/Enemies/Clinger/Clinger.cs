using Godot;
using System;

public partial class Clinger : Enemy
{

	[Export] Godot.Collections.Array<AudioStream> footsteps;
	[Export] Node3D neckPoint;
	[Export] ChainParticleTrigger torsoMist;

	[Export] protected PackedScene bloodSplatNeck;
    [Export] protected ChainParticleTrigger bloodMistNeck;
    [Export] protected PackedScene headNode;
	[Export] Node3D headPoint;
	[Export] Godot.Collections.Array<Tentacle> tentacles;

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

	int tendralItr = 0;

	public void ShootTendral()
	{
        tentacles[0].LaunchTendral();
        tentacles[1].LaunchTendral();
        tentacles[2].LaunchTendral();


		//tentacles[tendralItr].LaunchTendral();
		//tendralItr++;
		//if (tendralItr >= tentacles.Count)
		//{
		//	tendralItr = 0;
		//}

		torsoMist.Launch();
	}
}
