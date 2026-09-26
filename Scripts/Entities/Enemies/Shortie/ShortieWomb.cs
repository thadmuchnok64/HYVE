using Godot;
using System;

public partial class ShortieWomb : Enemy
{
	[Export] public int maximumChildren = 3;
	[Export] Node3D childrenParent;
	[Export] ChainParticleTrigger deathParticles;
	int currentChildren;
	public override void HitEnemyFromDirection(float damage, float postureDamage, SwingDirection dir)
	{
		TakeDamage(damage);
		TakePostureDamage(postureDamage);

		var inst = bloodSplatSmall.Instantiate();
		((Node3D)GetParent()).AddSibling(inst);
		((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;

		var state = currentState.HitEvent();
		if (state != null)
		{
			SwitchState(state);
		}
	}

	public override void Die()
	{
		var inst = bloodSplat.Instantiate();
		((Node3D)GetParent()).AddSibling(inst);
		((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;
		deathParticles.Launch();
		GoreManager.Instance.RequestGoreAtLocation(GoreType.FLESH,bloodPoint.GlobalPosition);
		GoreManager.Instance.RequestGoreAtLocation(GoreType.FLESH, bloodPoint.GlobalPosition);
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BRAIN, bloodPoint.GlobalPosition);
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BONE, bloodPoint.GlobalPosition);



		alive = false;

		//meshInstance.Mesh = headlessMesh;

	}

	public void IncrementChildren()
	{
		currentChildren++;
	}

	public bool canProduceMoreChildren()
	{
		currentChildren = 0;
		foreach(Node3D c in childrenParent.GetChildren())
		{
			if (c.GetChild(0) is Enemy && ((Enemy)c.GetChild(0)).alive)
				currentChildren++;
		}
		if (currentChildren >= maximumChildren) return false;
		return true;
	}
}
