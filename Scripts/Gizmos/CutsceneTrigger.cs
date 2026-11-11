using Godot;
using System;

public partial class CutsceneTrigger : AreaInteractable
{
	[Export] AnimationPlayer anim;
	[Export] string startingClip;
	[Export] Camera3D cutsceneCam;

	public override void BodyEntered(Node3D body)
	{
		base.BodyEntered(body);
		anim.Play(startingClip);
		cutsceneCam.Current = true;
		for (int i = 0; i < controllingStates.Count; i++)
		{
			controllingStates[i].anim.Active = false;
		}

	}
}
