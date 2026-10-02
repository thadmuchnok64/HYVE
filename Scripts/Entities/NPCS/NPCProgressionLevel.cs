using Godot;
using System;

public partial class NPCProgressionLevel : Resource
{
    [Export] Godot.Collections.Array<DA_DialogueTree> dialoguesThisLevel;
}
