using Godot;
using System;

public partial class CollectablePickup : InteractableObject
{
	[Export] Collectable heldCollectable;
	// Called when the node enters the scene tree for the first time.
	public override void TriggerGizmo(PCStateMachine pc)
	{
		pc.inventory.InsertCollectable(heldCollectable);
		Visible = false;
	}
}
