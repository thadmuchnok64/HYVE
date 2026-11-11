using Godot;
using System;

public partial class CutsceneManager : Node
{
	public static CutsceneManager instance;
	CutsceneTrigger trigger;
	AnimationTree anim;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(instance == null)
		{
			instance = this;
			return;
		}
		else
		{
			GD.Print("what the actual shit");
		}
	}

	public void TriggerCutscene(CutsceneTrigger requestingAnim)
	{
		if(IsOccupied())
		{
			GD.Print("Cutscene Manaager is already occupied!");
			return;
		}
		trigger = requestingAnim;
		anim = requestingAnim.anim;
		anim.Active = true;
		RenderingServer.GlobalShaderParameterSet("depthCulling", false);

	}
	
	public void CloseCutscene()
	{
		RenderingServer.GlobalShaderParameterSet("depthCulling", true);

	}


	public bool IsOccupied() { return anim != null; }

	public void ProgressCutsceneBreak(string animMeta)
	{
		anim.Set($"parameters/conditions/{animMeta}", true);
		trigger.canOpenDialogue = true;
	}
}
