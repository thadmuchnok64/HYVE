using Godot;
using System;

public partial class AmbientSoundPlayer : AudioStreamPlayer3D
{

	[Export] AudioStream audToPlay;

	[Export] bool looping = false;
	[Export] float minTimeForNextSound;
	[Export] float maxTimeForNextSound;
	float timer = 0;
	float cooldown = 0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (looping)
			PlaySound();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (looping)
			return;
		timer += (float)delta;
		if(timer > cooldown)
		{
			PlaySound();
		}
	}

	public void PlaySound()
	{
		Stream = audToPlay;
		Play();
		timer = 0;
		cooldown = minTimeForNextSound + GD.Randf() * (maxTimeForNextSound - minTimeForNextSound);
		
	}
}
