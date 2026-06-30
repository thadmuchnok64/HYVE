using Godot;
using System;

public partial class HUDManager : Control
{
	[Export] TextureProgressBar staminaBar;

	public static HUDManager instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (instance != null)
		{
			GD.Print("wtf");
		}
		else { instance = this; }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetStamina(float current, float max)
	{
		staminaBar.MaxValue = max;
		staminaBar.Value = current;
	}
}
