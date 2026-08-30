using Godot;
using System;
using System.Text;

public partial class DialogueContainer : Control
{
	[Export] RichTextLabel text;
	[Export] AnimationPlayer anim;
	[Export] PanelContainer panel;
	[Export] float timeForIncrement = .025f;
	float timer = 0;

	int itr = 0;
	string goalText;
	StringBuilder incrementalText = new StringBuilder();

	public override void _Process(double delta)
	{
		base._Process(delta);
		timer += (float)delta;
		if (goalText != null && timer > timeForIncrement&&!incrementalText.Equals(goalText))
		{
			timer = 0;
			IterateText();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		text.Text = "";
	}

	private void IterateText()
	{
		incrementalText.Append(goalText,itr,1);
		itr++;
		text.Text = incrementalText.ToString();
	}
	public void Populate(string _text)
	{
		itr = 0;
		goalText = _text;
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
