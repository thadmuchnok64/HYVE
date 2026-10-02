using Godot;
using System;

public partial class TimeManager : Node
{

	public static TimeManager instance;

	public override void _Ready()
	{
		if(instance != null)
		{
			GD.Print("wtf");
			return;
		}
		instance = this;
	}

	public async void Hitstop(float delay = .2f)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame); // wait one frame, (wait for spawning gore/heads/etc)

		RenderingServer.GlobalShaderParameterSet("impactFrame", true);
		//Engine.TimeScale = .1f;
		GetTree().Paused = true;
		await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
		RenderingServer.GlobalShaderParameterSet("impactFrame", false);
		GetTree().Paused = false;
		//Engine.TimeScale = 1f;



	}
}
