using Godot;
using System;

public partial class TorsoCrawler : Enemy
{
	[Export] Godot.Collections.Array<AudioStream> footsteps;
	[Export] Node3D backBone;
	[Export] GpuParticles3D bloodMist;

	public void Footstep()
	{
		SoundManager.Instance.RequesetSFXSoundAtLocation(footsteps.PickRandom(), GlobalPosition);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void BackBreak()
	{
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BONE,backBone.GlobalPosition);
        GoreManager.Instance.RequestGoreAtLocation(GoreType.FLESH, backBone.GlobalPosition);
		bloodMist.Emitting = true;

    }
}
