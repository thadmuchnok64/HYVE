using Godot;
using System;

[GlobalClass]
public partial class DA_DialogueResponse : DA_DialogueNode
{
	[Export] public string text;
	[Export] public DA_DialogueNode nextNode;
}