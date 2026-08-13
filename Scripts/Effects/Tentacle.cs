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
	[Export] Node3D tendralStretchPoint;
	[Export] Godot.Collections.Array<TentacleContact> contacts;
	[Export] Enemy enem;
	float timer;
	bool shot = false;

	Vector3 pinPoint;

	public override void _Ready()
	{
		base._Ready();
		foreach (var contact in contacts)
		{
			contact.enemyRef = enem;
			contact.tentacle = this;
		}
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

	public override void FixScale()
	{

		if (!retracting)
		{
			var pos = tendralStretchPoint.GlobalPosition;
			var dis = pos.DistanceTo(marker.GlobalPosition);
			var scale = dis / baseLength;
			scale = (float)Mathf.Clamp(scale, 1f, 5f);
			//GD.Print(scale);

            skeleton.SetBonePoseScale(0, Vector3.One);
            skeleton.SetBonePoseScale(1, Vector3.One);

            skeleton.SetBonePoseScale(2, Vector3.One.ReplaceY(scale));
		}
		else
		{
			var pos = tendralStretchPoint.GlobalPosition;
			var dis = pos.DistanceTo(marker.GlobalPosition);
			var scale = dis / baseLength;
			scale = (float)Mathf.Clamp(scale, .05f, 5f);
			//GD.Print(scale);
            skeleton.SetBonePoseScale(0, Vector3.One * (1f-(timer / timeToRetract)).Clamp01());
            skeleton.SetBonePoseScale(2, Vector3.One.ReplaceY(scale));
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
        foreach (var contact in contacts)
        {
			contact.SetWeaponActive(true);
        }
        shot = true;
	}

	public void Retract()
	{
		retracting = true;
		timer = 0;
		shot = false;
		SoundManager.Instance.RequesetSFXSoundAtLocation(retractSound, pinPoint);
	}

	public bool CanAttack()
	{
		return (!retracting && shot);
	}

	
}
