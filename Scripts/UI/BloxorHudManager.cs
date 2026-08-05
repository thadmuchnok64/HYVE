using Godot;
using System;

public partial class BloxorHudManager : SubViewport
{

	[Export] RichTextLabel levelText;

	public void SetLevel(int lev)
	{
		levelText.Text = $"Level {lev}";
	}
}
