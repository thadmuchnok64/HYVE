using Godot;
using System;
using System.Diagnostics;
using System.Net;

public partial class Centaur : Enemy
{
	 
	[Export] protected Mesh headlessMesh2;
	[Export] Node3D headPoint;
	[Export] Node3D head2Point;
	[Export] Node3D bloodpoint2;
	[Export] protected PackedScene bloodSplatNeck;
	[Export] protected ChainParticleTrigger bloodMistNeck, bloodMistTail;
	[Export] protected PackedScene headNode;
	[Export] int firstDecapitationThreshhold = 100;
	bool decapitated = false;

	public bool CheckForPrimaryDecapitation()
	{
		if (decapitated)
			return false;
		if(health <= firstDecapitationThreshhold)
		{
			var inst = bloodSplat.Instantiate();
			cb.AddSibling(inst);
			((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;
			meshInstance.Mesh = headlessMesh2;
			decapitated = true;
			return true;
		}
		return false;

	}

	public void NeckSprayBlood()
	{
		var inst = bloodSplatNeck.Instantiate();
		cb.AddSibling(inst);
		((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;
		((Node3D)inst).GlobalRotation = bloodPoint.GlobalRotation;

	}

	public void BloodMistNeck()
	{
		bloodMistNeck.Launch();
		/*
		var inst = headNode.Instantiate();
		cb.AddSibling(inst);
		((Node3D)inst).GlobalPosition = headPoint.GlobalPosition;
		((Node3D)inst).GlobalRotation = headPoint.GlobalRotation;
		((PhysicalParticleLauncher)inst).Launch();
		*/
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BRAIN, bloodPoint.GlobalPosition);
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BRAIN, bloodPoint.GlobalPosition);

	}

	public void NeckSprayBloodHead2()
	{
		var inst = bloodSplatNeck.Instantiate();
		cb.AddSibling(inst);
		((Node3D)inst).GlobalPosition = bloodpoint2.GlobalPosition;
		((Node3D)inst).GlobalRotation = bloodpoint2.GlobalRotation;

	}

	public void BloodMistNeckHead2()
	{
		bloodMistTail.Launch();
		/*
		var inst = headNode.Instantiate();
		cb.AddSibling(inst);
		((Node3D)inst).GlobalPosition = headPoint.GlobalPosition;
		((Node3D)inst).GlobalRotation = headPoint.GlobalRotation;
		((PhysicalParticleLauncher)inst).Launch();
		*/
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BRAIN, bloodpoint2.GlobalPosition);
		GoreManager.Instance.RequestGoreAtLocation(GoreType.BRAIN, bloodpoint2.GlobalPosition);

	}

}
