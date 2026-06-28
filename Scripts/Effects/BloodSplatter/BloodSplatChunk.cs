using Godot;
using System;

public partial class BloodSplatChunk : Node3D
{
	[Export] PackedScene bloodSplatDecal;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Contact(Node body)
	{
		GD.Print("stuck");
		var inst = bloodSplatDecal.Instantiate();
		AddSibling(inst);
		((Node3D)inst).GlobalPosition = GlobalPosition;

	}
}
