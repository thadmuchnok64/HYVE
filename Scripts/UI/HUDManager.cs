using Godot;
using System;

public partial class HUDManager : Control
{
	[Export] TextureProgressBar staminaBar;
	[Export] TextureProgressBar healthBar;
	[Export] TextureProgressBar posBar;


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

	#region Bars
	public void SetStamina(float current, float max)
	{
		staminaBar.MaxValue = max;
		staminaBar.Value = current;
	}

	public void SetHealth(float current, float max)
	{
		healthBar.MaxValue = max;
		healthBar.Value = current;
	}

	public void SetPosture(float current, float max)
	{
		posBar.MaxValue = max;
		posBar.Value = current;
	}
	#endregion

}
