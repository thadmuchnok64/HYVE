using Godot;
using System;

public partial class TorsoCrawler : Enemy
{
	[Export] Godot.Collections.Array<AudioStream> footsteps;

	public void Footstep()
	{
		SoundManager.Instance.RequesetSFXSoundAtLocation(footsteps.PickRandom(), GlobalPosition);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
