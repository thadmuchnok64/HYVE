using Godot;
using System;
using System.Collections.Generic;


public partial class HamsterWheel : AreaInteractable
{
	[Export] float radius;
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
			var speed =(controllingStates[0].cb.Velocity * Basis.Z).Length()*2*Mathf.Pi*radius*(float)delta;
			var newRot = Rotation.X + speed;
			if(Mathf.RadToDeg(newRot) > 360)
			{
				newRot = newRot - (2*Mathf.Pi);
			}
			if (Mathf.RadToDeg(newRot) < -360)
			{
				newRot = Mathf.DegToRad(newRot + (2 * Mathf.Pi));
			}

				Rotation = new Vector3(newRot, Rotation.Y, Rotation.Z);
		}
	}
}
