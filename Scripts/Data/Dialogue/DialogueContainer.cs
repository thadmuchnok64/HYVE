using Godot;
using System;

public partial class DialogueContainer : Control
{
	[Export] RichTextLabel text;
	[Export] AnimationPlayer anim;
	[Export] PanelContainer panel;
	// Called when the node enters the scene tree for the first time.

	public void Populate(string _text)
	{
		text.Text = _text;
		FadeIn();
	}

	public void FadeIn()
	{
		anim.Play("FadeIn");
	}

	public void FadeOut()
	{
		anim.Play("FadeOut");
	}

	public void Select()
	{
		anim.Play("Select");
	}

	public void Unselect()
	{
		anim.Play("Unselect");
	}
}
