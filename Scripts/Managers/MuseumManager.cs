using Godot;
using System;


/// <summary>
/// The purpose of this script is to store values that can be reached from anywhere in the museum, even if I unload certain rooms
/// </summary>
public partial class MuseumManager : Node
{
	public static MuseumManager instance;

	[Export] float flowInPerSec = 1;
	[Export] float flowOutPerSec = .05f;

	bool airflowIn = false;
	public float airPressure = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			GD.Print("multiple museum managers, ya dingus");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ManagePressureDelta((float)delta);
	}

	public void LungPressure()
	{
		airflowIn = true;
	}

	private void ManagePressureDelta(float delta)
	{
		if (airflowIn)
		{
			airPressure += flowInPerSec * delta;
			if (airPressure >= 1)
				airflowIn = false;
		}
		else
		{
			if(airPressure > 0)
				airPressure -= flowOutPerSec * delta;
		}
	}
}
