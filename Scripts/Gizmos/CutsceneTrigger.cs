using Godot;
using System;

public partial class CutsceneTrigger : AreaInteractable
{
	[Export] public  AnimationTree anim;
	[Export] public AnimationPlayer animPlayer;
	[Export] Camera3D cutsceneCam;
	[Export] DA_DialogueTree connectedDialogue;
	[Export] Node3D characterPivot;
	[Export] Node3D sierraCutscenePivot;
	public bool canOpenDialogue = true;

    public override void _Ready()
    {
		base._Ready();
		animPlayer.Pause();
		
    }
	public override void BodyEntered(Node3D body)
	{
		base.BodyEntered(body);
		cutsceneCam.Current = true;
		for (int i = 0; i < controllingStates.Count; i++)
		{
			controllingStates[i].anim.Active = false;
		}
		CutsceneManager.instance.TriggerCutscene(this);
		
		anim.Set($"parameters/conditions/{animationMeta}", true);
		var statemachine = anim.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
		statemachine.Travel("Start");
		animPlayer.Play();
	}

	public void TriggerNextDialogue()
	{
		if (canOpenDialogue)
		{
			if (HUDManager.instance.IsDialogueOpen())
			{
				canOpenDialogue = false;
				HUDManager.instance.ProgressDialogueViaCutsceneBreak();
			}
			else
			{
				if (connectedDialogue != null)
				{
					canOpenDialogue = false;

					HUDManager.instance.RequestDialogue(connectedDialogue, characterPivot,sierraCutscenePivot);
				}
			}
		}
	}
}
