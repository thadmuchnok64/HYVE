using Godot;
using System;
using System.Collections.Generic;

public partial class SoundManager : Node3D
{
	public static SoundManager Instance;

	List<AudioStreamPlayer3D> sfxPool;
	AudioStreamPlayer3D currentPlayer;
	int currentSFXPlayer = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// singleton
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GD.Print("wtf");
			return;
		}
		// audio
			sfxPool = new List<AudioStreamPlayer3D>();
		foreach (AudioStreamPlayer3D audi in GetChildren())
		{
			sfxPool.Add(audi);
		}
		currentPlayer = sfxPool[0];
	}


	public void RequesetSFXSoundAtLocation(AudioStream sfx, Vector3 globalPos)
	{
		currentPlayer.GlobalPosition = globalPos;
		currentPlayer.Stream = sfx;
		currentPlayer.Play();
		FindNextSFXPlayer();
	}

	private void FindNextSFXPlayer()
	{
		currentSFXPlayer++;
		if(currentSFXPlayer >= sfxPool.Count)
		{
			currentSFXPlayer = 0;
		}
		currentPlayer = sfxPool[currentSFXPlayer];
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
