using Godot;
using System;

public partial class Tentacle : Tendral
{
	TentacleProjectile projectile;
	[Export] float timeToReach = .5f;
	[Export] PackedScene projectilePrefab;
	[Export] protected Fabrik3D ik;
	[Export] float retractAngle = Mathf.Pi / 3;
	bool retracting = false;
	[Export] float timeToRetract = .3f;
	[Export] bool debug = false;
	[Export] AudioStream retractSound;
	float timer;
	bool shot = false;

	Vector3 pinPoint;

	public override void _Ready()
	{
		base._Ready();
		//projectile.Launch(Vector3.Up);
	}
	public void HitEvent(Projectile proj)
	{
		if (retracting)
			return;
		pinPoint = projectile.GlobalPosition;
		proj.Freeze = true;
	}

	public override void _Process(double delta)
	{
		if (retracting)
		{
			timer += (float)delta;
			if(projectile != null)
			{
				marker.GlobalPosition = (pinPoint.Lerp(GlobalPosition, (timer / timeToRetract).Clamp01()));
			}
			if (timer / timeToRetract > 1)
			{
				retracting = false;
				Visible = false;
			}
		} else
		{
			if (shot && projectile != null&&projectile.Freeze)
			{
				if(GlobalBasis.Z.AngleTo((pinPoint - GlobalPosition).Normalized()) > retractAngle)
				{
					Retract();
				}
			}
		}
			base._Process(delta);
		if(projectile != null && !projectile.Freeze)
		{
			marker.GlobalPosition = projectile.GlobalPosition;
		}
		else
		{
			marker.GlobalPosition = pinPoint;
		}
	}

	public void LaunchTendral()
	{
		if (retracting)
			return;
		if(projectile == null)
		projectile = (TentacleProjectile)(projectilePrefab.Instantiate());
		Visible = true;
		ProjectileManager.instance.AddChild(projectile);
		projectile.tentacle = this;
		projectile.GlobalPosition = GlobalPosition;
		projectile.Launch(GlobalBasis.Z);
		shot = true;
	}

	public void Retract()
	{
		retracting = true;
		timer = 0;
		shot = false;
		SoundManager.Instance.RequesetSFXSoundAtLocation(retractSound, pinPoint);
	}

	
}
