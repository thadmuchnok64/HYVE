using Godot;
using System;
using System.Diagnostics;

public partial class DialogueState : UIState
{
	public DA_DialogueTree loadedDialogue;
	DA_DialogueNode currentNode;
	int dialogueChoiceIndex = 0;
	public Node3D npcPivot;

	PCStateMachine pc;
	[Export] PackedScene dialogueBubble;
	[Export] Control dialogueContainer;

	[Export] AudioStream bubbleInSFX;
	[Export] AudioStream extendSFX;
	public override UIState Enter()
	{
		base.Enter();
		pc = ((PCStateMachine)GameMaster.Instance.GetPlayer().GetChild(0)); // bandaid. clean this up later
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
		ProgressDialogue();
		return null;
	}

	private void DestroyOldDialogue()
	{
		foreach (var child in dialogueContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void SetupNPCDialogue(DA_DialogueNPCStatement statement)
	{
		DestroyOldDialogue();
		var bubble = dialogueBubble.Instantiate();
		dialogueContainer.AddChild(bubble);
		((DialogueContainer)bubble).Populate(statement.text);
		dialogueContainer.Position = GameMaster.Instance.mainCamRef.UnprojectPosition(npcPivot.GlobalPosition) - (dialogueContainer.Size / 2f);// - new Vector2(0, trackerOffset);
		HUDManager.instance.PlaySound(bubbleInSFX);
	}

	public void SetupBranch(DA_DialogueBranch branch)
	{
		dialogueChoiceIndex = 0;
		DestroyOldDialogue();
		foreach (var response in branch.responses)
		{
			var bubble = dialogueBubble.Instantiate();
			dialogueContainer.AddChild(bubble);
			((DialogueContainer)bubble).Populate(response.text);
		}
		((DialogueContainer)dialogueContainer.GetChild(0)).Select();
		dialogueContainer.Position = (GameMaster.Instance.mainCamRef.UnprojectPosition(pc.trackingPoint.GlobalPosition)) - (dialogueContainer.Size / 2f);// - new Vector2(0, trackerOffset);
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

	public void ProgressDialogue()
	{
		if(currentNode is DA_DialogueNPCStatement)
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

	}




}
