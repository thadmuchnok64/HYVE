using Godot;
using System;
using System.Collections.Generic;


public partial class HamsterWheel : AreaInteractable
{
	[Export] float radius;
	[Export] Node3D pivot;
    [Export] Godot.Collections.Array<Node3D> extraPivots;
    [Export] Godot.Collections.Array<float> extraRatios;

    [Export] float speedLossPerSec = .25f;
	float speed = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controllingStates = new List<PCStateMachine>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (controllingStates.Count > 0)
		{
			controllingStates[0].cb.GlobalPosition = controllingStates[0].cb.GlobalPosition.ReplaceZ(GlobalPosition.Z);
			speed = (controllingStates[0].cb.Velocity * Basis.Z).Length() * 2 * Mathf.Pi * radius * (float)delta;
			speed *= -Mathf.Sign(controllingStates[0].cb.Velocity.Z);

			SpinWheel();
		}
		else if (Mathf.Abs(speed) > .0001f)
		{
			speed = speed * (Mathf.Lerp(1, speedLossPerSec, (float)delta));
			SpinWheel();
		}
	}

	private void SpinWheel()
	{
        var newRot = pivot.Rotation.X + speed;
        pivot.Rotation = new Vector3(newRot, pivot.Rotation.Y, pivot.Rotation.Z);

		int i = 0;
		foreach(Node3D p in extraPivots)
		{
			p.Rotation = new Vector3(newRot * extraRatios[i], pivot.Rotation.Y, pivot.Rotation.Z);

            i++;
		}
    }
}
