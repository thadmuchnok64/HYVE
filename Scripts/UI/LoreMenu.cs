using Godot;
using System;

public partial class LoreMenu : UIState
{
	[Export] RichTextLabel mainContent;
	public void Populate(LoreNote note)
	{
		mainContent.Text = note.NoteContent;
	}
}
