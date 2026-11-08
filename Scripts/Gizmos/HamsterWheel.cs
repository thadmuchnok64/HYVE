using Godot;
using System;
using System.Collections.Generic;


public partial class HamsterWheel : AreaInteractable
{
	[Export] float radius;
	[Export] Node3D pivot;
    [Export] Godot.Collections.Array<Node3D> extraPivots;
    [Export] Godot.Collections.Array<float> extraRatios;
	[Export] AudioStreamPlayer3D aud;
	[Export] AudioStream clickSFX;
	[Export] Node3D platform;
	[Export] MeshInstance3D ropeMesh;
	[Export] float platformMinY = 0;
	[Export] float platformMaxY = 3.322f;
	[Export] ChainParticleTrigger platformParticles;
	[Export] AudioStream platformSettleSound;
	[Export] Node3D missingGear;
	float platformDelta = 0;
	bool hasMissingGear = false;
	bool platformLowered = false;


	[Export] float speedLossPerSec = .25f;
	float speed = 0;
	float currentSubangle = 0;
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
		if(hasMissingGear)
		ManagePlatform(speed*(float)delta);

	}

	private void SpinWheel()
	{
        var newRot = pivot.Rotation.X + speed;
        pivot.Rotation = new Vector3(newRot, pivot.Rotation.Y, pivot.Rotation.Z);

		int i = 0;
		foreach(Node3D p in extraPivots)
		{
			if (!p.Visible)
			{
				goto exit;
			}
			p.Rotation = new Vector3(newRot * extraRatios[i], pivot.Rotation.Y, pivot.Rotation.Z);
            i++;
		}
		exit:
		if(Mathf.Floor(pivot.RotationDegrees.X/45f) != currentSubangle)
		{
			currentSubangle = Mathf.Floor(pivot.RotationDegrees.X / 45f);
			aud.Stream = clickSFX;
			aud.Play();
        }
    }

	private void ManagePlatform(float delta)
	{
		platformDelta = (platformDelta + (float)delta*10f).Clamp01();
		platform.Position = platform.Position.ReplaceY(Mathf.Lerp(platformMaxY,platformMinY,platformDelta));
		ropeMesh.SetBlendShapeValue(0, 1 - platformDelta);
		if(platformLowered == false && platformDelta > .99f)
		{
			platformLowered = true;
			platformParticles.Launch();
			SoundManager.Instance.RequesetSFXSoundAtLocation(platformSettleSound, platform.GlobalPosition,2f);
		} else if (platformDelta < .98f)
		{
			platformLowered = false;
		}
	}

	public async void InsertGear()
	{
		await ToSignal(GetTree().CreateTimer(.4f), SceneTreeTimer.SignalName.Timeout);

		hasMissingGear = true;
		missingGear.Visible = true;
		SoundManager.Instance.RequesetSFXSoundAtLocation(platformSettleSound, missingGear.GlobalPosition,1.5f);
	}

}
