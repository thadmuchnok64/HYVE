using Godot;
using System;

public partial class NucleusDoor : Node3D
{
	[Export] AnimationPlayer anim;
	bool open = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (open)
		{
			if(MuseumManager.instance.airPressure < .5f)
			{
				open = false;
				anim.Play("close");
			}
		}
		else
		{
			if (MuseumManager.instance.airPressure > .5f)
			{
				open = true;
				anim.Play("open");
			}
		}
	}
}
