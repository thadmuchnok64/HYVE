using Godot;
using System;

public partial class CollectablePickup : InteractableObject
{
	[Export] Collectable heldCollectable;
	[Export] AudioStream soundOnCollect;
	// Called when the node enters the scene tree for the first time.
	public override void TriggerGizmo(PCStateMachine pc)
	{
		pc.inventory.InsertCollectable(heldCollectable);
		if(soundOnCollect!= null)
		SoundManager.Instance.RequesetSFXSoundAtLocation(soundOnCollect, GlobalPosition);
		Visible = false;
	}
}
