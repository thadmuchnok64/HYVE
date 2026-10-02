using Godot;
using System;

public partial class NPCEntity : Entity
{
	[Export] public int progressionValue;
	[Export] Godot.Collections.Array<NPCProgressionLevel> dialogueProgression; // lets you assign a list of dialogues for each progression level

	public DA_DialogueTree getCurrentDialogue()
	{
		return (DA_DialogueTree)dialogueProgression[progressionValue].GetNextDialogue();
	}

}
