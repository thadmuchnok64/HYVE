using Godot;
using System;

public partial class CutsceneTrigger : AreaInteractable
{
	[Export] public  AnimationTree anim;
	[Export] Camera3D cutsceneCam;
	public bool canOpenDialogue = true;

    public override void _Ready()
    {
		anim.Active = false;
    }
	public override void BodyEntered(Node3D body)
	{
		base.BodyEntered(body);
		cutsceneCam.Current = true;
		for (int i = 0; i < controllingStates.Count; i++)
		{
			controllingStates[i].anim.Active = false;
		}
		CutsceneManager.instance.TriggerCutscene(anim);

	}

	public void TriggerNextDialogue()
	{
		if (canOpenDialogue)
		{
			canOpenDialogue = false;
			c
		}
	}
}
