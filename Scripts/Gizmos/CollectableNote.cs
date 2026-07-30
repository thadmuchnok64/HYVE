using Godot;
using System;

public partial class CollectableNote : InteractableObject
{
	[Export]  LoreNote note;
	// Called when the node enters the scene tree for the first time.
	public override void TriggerGizmo(PCStateMachine pc)
	{
		Visible = false;
		HUDManager.instance.ToggleLore(note);
	}
}
