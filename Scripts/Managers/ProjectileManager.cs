using Godot;
using System;

public partial class ProjectileManager : Node3D
{

	public static ProjectileManager instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (instance != null)
		{
			GD.Print("what the fuck");
		}
		else { instance = this; }
	}

}
