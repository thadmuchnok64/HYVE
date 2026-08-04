using Godot;
using System;

public partial class BloxorPC : RigidBody3D
{
	[Export] RayCast3D ray1, ray2;
	[Export] float sideForce = 5;

	public bool CheckIfSpotIsValid()
	{
		return true;
		ray1.GlobalRotation = Vector3.Zero;
		ray2.GlobalRotation = Vector3.Zero;
		bool ray1success = ray1.IsColliding();
		bool ray2success = ray2.IsColliding();


		if (ray1success && ray2success)
			return true;
		else
		{
			Freeze = false;
			if (!ray1success)
			{
				ApplyImpulse(sideForce * Vector3.Down, ray1.GlobalPosition - GlobalPosition);
				GD.Print("uh");
			}
			if (!ray2success)
			{
				ApplyImpulse(sideForce * Vector3.Down, ray2.GlobalPosition - GlobalPosition);
				GD.Print("wtf");

			}

			return false;
		}
	}
}
