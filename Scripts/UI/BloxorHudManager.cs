using Godot;
using System;

public partial class BloxorHudManager : SubViewport
{

	[Export] RichTextLabel levelText;

	public void SetLevel(int lev)
	{
		levelText.Text = $"[shake rate=8 level=2 connected=0]LEVEL {lev}[/shake]";
	}
}
