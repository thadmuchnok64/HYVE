using Godot;
using System;

public partial class DA_DialogueBranch : DA_DialogueNode
{
	[Export] public Godot.Collections.Array<DA_DialogueResponse> responses;

	public DA_DialogueBranch()
	{
		responses = new Godot.Collections.Array<DA_DialogueResponse> ();
	}
}
