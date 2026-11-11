using Godot;
using System;

[GlobalClass]

public partial class DA_CutsceneBreak : DA_DialogueNode
{
    [Export] public string cutsceneMeta;
    [Export] public DA_DialogueNode nextNode;
}
