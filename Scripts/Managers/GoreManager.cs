using Godot;
using System;


public enum GoreType { FLESH, BONE, BRAIN}
public partial class GoreManager : Node3D
{
	[Export] float launchVelocity = 4f;
	[Export] Godot.Collections.Array<PackedScene> fleshPrefabs;
	[Export] Godot.Collections.Array<PackedScene> bonePrefabs;
	[Export] Godot.Collections.Array<PackedScene> brainPrefabs;

	public static GoreManager Instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(Instance != null)
		{
			GD.Print("Multiple goremanagers!!! wtf is wrong with you");
			return;
		}
		Instance = this;
	}


	public void RequestGoreAtLocation(GoreType goreType, Vector3 globalPosition)
	{
		switch (goreType)
		{
			case GoreType.FLESH:
				LaunchGore(fleshPrefabs.PickRandom(), globalPosition);
				break;
			case GoreType.BONE:
				LaunchGore(bonePrefabs.PickRandom(), globalPosition);
				break;
			case GoreType.BRAIN:
				LaunchGore(brainPrefabs.PickRandom(), globalPosition);
				break;
		}
	}

	public void LaunchGore(PackedScene prefab, Vector3 globalPosition)
	{
		var gore = prefab.Instantiate();
		AddChild(gore);
		((Node3D)gore).GlobalPosition = globalPosition;
		((RigidBody3D)gore).LinearVelocity = StaticHelpers.RandomVector() * launchVelocity;
	}
}
