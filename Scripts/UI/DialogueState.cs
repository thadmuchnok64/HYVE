using Godot;
using System;
using System.Diagnostics;

public partial class DialogueState : UIState
{
	public DA_DialogueTree loadedDialogue;
	DA_DialogueNode currentNode;
	int dialogueChoiceIndex = 0;
	public Node3D npcPivot;
	[Export] AudioStreamPlayer2D textDialogueAud;

	PCStateMachine pc;
	[Export] PackedScene dialogueBubble;
	[Export] Control dialogueContainer;
	[Export] Control dialoguePivot;

	[Export] AudioStream bubbleInSFX;
	[Export] AudioStream extendSFX;
	[Export] AudioStream aliceVocals;
	[Export] AudioStream ernieVocals; // temporary. Switch this out for whatever the dialogue needs

	bool branchOpen = false;

	public override UIState Enter()
	{
		base.Enter();
		pc = ((PCStateMachine)GameMaster.Instance.GetPlayer().GetChild(0)); // ugly bandaid. clean this up later maybe
		InitializeDialogue();
		return this;
	}

	private async void InitializeDialogue()
	{
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
		currentNode = loadedDialogue.startingNode;

		if (currentNode is DA_DialogueNPCStatement)
			SetupNPCDialogue((DA_DialogueNPCStatement)currentNode);

	}

	public override UIState Exit()
	{
		pc.SetDefaultCamPoint();
		return base.Exit();
	}

	public override UIState NavForward()
	{
		if (branchOpen)
		{
			HUDManager.instance.ConfirmSound();
		}
		ProgressDialogue();
		return null;
	}

	private async void DestroyOldDialogue()
	{
		foreach (var child in dialogueContainer.GetChildren())
		{
			((DialogueContainer)child).FadeOut();
		}

		await ToSignal(GetTree().CreateTimer(.2f), SceneTreeTimer.SignalName.Timeout);

		foreach (var child in dialogueContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	public async void SetupNPCDialogue(DA_DialogueNPCStatement statement)
	{
		DestroyOldDialogue();
		await ToSignal(GetTree().CreateTimer(.2f), SceneTreeTimer.SignalName.Timeout);
		var bubble = dialogueBubble.Instantiate();
		dialogueContainer.AddChild(bubble);
		((DialogueContainer)bubble).Populate(statement.text, textDialogueAud,ernieVocals);
		dialoguePivot.Position = GameMaster.Instance.mainCamRef.UnprojectPosition(npcPivot.GlobalPosition) - (dialogueContainer.Size / 2f);// - new Vector2(0, trackerOffset);
		HUDManager.instance.PlaySound(bubbleInSFX);
	}

	public async void SetupBranch(DA_DialogueBranch branch)
	{
		dialogueChoiceIndex = 0;
		DestroyOldDialogue();
		await ToSignal(GetTree().CreateTimer(.22f), SceneTreeTimer.SignalName.Timeout);
		DialogueContainer first = null;
		foreach (var response in branch.responses)
		{
			var bubble = dialogueBubble.Instantiate();
			dialogueContainer.AddChild(bubble);
			bool multi = branch.responses.Count > 1;
			((DialogueContainer)bubble).Populate(response.text,textDialogueAud,aliceVocals,multi);
			if(first == null)
			{
				first = (DialogueContainer)bubble;
				first.Select();
			}
		}
		if (branch.responses.Count > 1)
		{
			branchOpen = true;
			HUDManager.instance.ToggleDialogueBranch(true);
		}
		dialoguePivot.Position = (GameMaster.Instance.mainCamRef.UnprojectPosition(pc.trackingPoint.GlobalPosition)) - (dialogueContainer.Size / 2f);// - new Vector2(0, trackerOffset);
		HUDManager.instance.PlaySound(extendSFX);

	}

	public override UIState NavUp()
	{
		if (currentNode is DA_DialogueBranch)
		{
			var len = ((DA_DialogueBranch)currentNode).responses.Count - 1;
			var oldIndex = dialogueChoiceIndex;
			dialogueChoiceIndex = Math.Clamp(dialogueChoiceIndex - 1, 0, len);
			if(dialogueChoiceIndex != oldIndex)
			{
				((DialogueContainer)dialogueContainer.GetChild(oldIndex)).Unselect();
				((DialogueContainer)dialogueContainer.GetChild(dialogueChoiceIndex)).Select();
				HUDManager.instance.NavigationSound();

			}

		}
		return base.NavUp();
	}

	public override UIState NavDown()
	{
		if (currentNode is DA_DialogueBranch)
		{
			var len = ((DA_DialogueBranch)currentNode).responses.Count - 1;
			var oldIndex = dialogueChoiceIndex;
			dialogueChoiceIndex = Math.Clamp(dialogueChoiceIndex + 1, 0, len);
			if (dialogueChoiceIndex != oldIndex)
			{
				((DialogueContainer)dialogueContainer.GetChild(oldIndex)).Unselect();
				((DialogueContainer)dialogueContainer.GetChild(dialogueChoiceIndex)).Select();
				HUDManager.instance.NavigationSound();
			}
		}
		return base.NavDown();

	}

	public void SetupNext(DA_DialogueNode node)
	{
		if (node is DA_DialogueNPCStatement)
			SetupNPCDialogue((DA_DialogueNPCStatement)node);
		else if (node is DA_DialogueBranch)
			SetupBranch((DA_DialogueBranch)node);
	}

	public async void ProgressDialogue()
	{
		if (branchOpen)
		{
			branchOpen = false;
			HUDManager.instance.ToggleDialogueBranch(false);
			await ToSignal(GetTree().CreateTimer(.2f), SceneTreeTimer.SignalName.Timeout);
		}
		if (currentNode is DA_DialogueNPCStatement)
		{
			//animate
			currentNode = ((DA_DialogueNPCStatement)currentNode).nextNode;
		}else if (currentNode is DA_DialogueBranch)
		{
			var branch = (DA_DialogueBranch)currentNode;
			if (branch.responses.Count == 1)
			{
				currentNode = branch.responses[0].nextNode;
			}
			else
			{
				currentNode = branch.responses[dialogueChoiceIndex].nextNode;

			}
		}

		if(currentNode == null)
		{
			await ToSignal(GetTree().CreateTimer(.2f), SceneTreeTimer.SignalName.Timeout);
			ExitDialogue();
		}
		else
		{
			SetupNext(currentNode);
		}
	}

	public void ExitDialogue()
	{
		DestroyOldDialogue();
		HUDManager.instance.SwitchState(null);
		pc?.ForceUninteract();

	}




}
