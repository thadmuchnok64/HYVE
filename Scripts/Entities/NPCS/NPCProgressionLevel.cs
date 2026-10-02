using Godot;
using System;
using System.Linq;

[Tool]
[GlobalClass]
public partial class NPCProgressionLevel : Resource
{
    [Export] Godot.Collections.Array<DA_DialogueTree> dialoguesThisLevel;
    int dialogueIndex = 0;

    public NPCProgressionLevel()
    {
		dialoguesThisLevel = new Godot.Collections.Array<DA_DialogueTree>();
		dialogueIndex = 0;
	}
    public NPCProgressionLevel(Godot.Collections.Array<DA_DialogueTree> dialogues)
    {
        dialoguesThisLevel = dialogues;
        dialogueIndex = 0;
    }

    public DA_DialogueTree GetNextDialogue()
    {
        DA_DialogueTree foundDialogue;
        if(dialogueIndex >= dialoguesThisLevel.Count)
        {
			foundDialogue = dialoguesThisLevel.Last();
        }
        else
        {
			foundDialogue = dialoguesThisLevel[dialogueIndex];
        }
        dialogueIndex++;
        return foundDialogue;
	}


}
