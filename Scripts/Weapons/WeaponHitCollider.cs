using Godot;
using System;
using System.Collections.Generic;

public partial class WeaponHitCollider : Area3D
{
	[Export] public AttackType attackType;
	/*
	private List<Node3D> overlappingObjects;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		overlappingObjects = new List<Node3D>();
	}

	public List<Node3D> GetOverlappingObjects()
	{
		return overlappingObjects;
	}

	public void ObjectEnter(Node3D body)
	{
		if(overlappingObjects.Contains(body))
			return;
		overlappingObjects.Add(body);
	}

	public void ObjectExit(Node3D body)
	{
		if (overlappingObjects.Contains(body))
		overlappingObjects.Remove(body);
	}
	*/
}
