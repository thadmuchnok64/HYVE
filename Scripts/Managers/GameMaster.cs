using Godot;
using System;

public partial class GameMaster : Node3D
{
	[Export] CharacterBody3D player;



	public static GameMaster Instance;
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{

		if (Instance != null)
		{
			GD.PrintErr("WTF MULTIPLE GAME MASTERS!!!");
		}
		else
		{
			Instance = this;
		}
	}

	public CharacterBody3D GetPlayer()
	{
		return player;
	}
}
