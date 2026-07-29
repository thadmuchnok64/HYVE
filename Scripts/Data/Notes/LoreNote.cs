using Godot;
using System;

[GlobalClass] public partial class LoreNote : Resource
{
	[Export] public string NoteTitle;
	[Export(PropertyHint.MultilineText)] public string NoteContent;
}
