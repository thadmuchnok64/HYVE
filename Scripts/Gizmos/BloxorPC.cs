using Godot;
using System;

public partial class BloxorPC : RigidBody3D
{
	[Export] RayCast3D ray1, ray2;
	[Export] float sideForce = 5;
	Vector3 ogPos, ogRot;
    public override void _Ready()
    {
        base._Ready();
		ogPos = GlobalPosition;
		ogRot = GlobalRotation;
    }
    public override void _PhysicsProcess(double delta)
    {

        ray1.GlobalRotation = Vector3.Zero;
        ray2.GlobalRotation = Vector3.Zero;
    }
	public bool CheckIfSpotIsValid()
	{
		bool ray1success = ray1.IsColliding();
		bool ray2success = ray2.IsColliding();


		if (ray1success && ray2success)
		{
            Freeze = true;
            return true;
		}
		else
		{
			Freeze = false;
			if (!ray1success)
			{
				ApplyImpulse(sideForce * Vector3.Down, ray1.GlobalPosition - GlobalPosition);
			}
			if (!ray2success)
			{
				ApplyImpulse(sideForce * Vector3.Down, ray2.GlobalPosition - GlobalPosition);

			}

			return false;
		}
	}

	public bool CheckIfWon()
	{
		bool ray1success = ray1.IsColliding();
		bool ray2success = ray2.IsColliding();
		if (ray1success && ray2success)
		{
			if (ray1.GetCollider() is BloxorWinBlock && ray2.GetCollider() is BloxorWinBlock)
			{
                Freeze = false;
                ApplyImpulse(sideForce * Vector3.Down, ray2.GlobalPosition - GlobalPosition);
                ApplyImpulse(sideForce * Vector3.Down, ray2.GlobalPosition - GlobalPosition);
                return true;
			}
		}
		return false;
	}

    public void Respawn()
	{
		Freeze = true;
		GlobalRotation = ogRot;
		GlobalPosition = ogPos;
	} 
}
