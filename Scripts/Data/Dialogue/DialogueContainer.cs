using Godot;
using System;
using System.Text;

public partial class DialogueContainer : Control
{
	[Export] RichTextLabel text;
	[Export] AnimationPlayer anim;
	[Export] PanelContainer panel;
	[Export] float timeForIncrement = .025f;
	public AudioStreamPlayer2D aud;
	AudioStream vocalFX;
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
		if (aud != null && vocalFX != null)
		{
			aud.Stream = vocalFX;
			aud.Play();
		}
	}
	public void Populate(string _text,AudioStreamPlayer2D _aud = null,AudioStream vocalSFX = null,bool instant = false)
	{
		itr = 0;
		goalText = _text;
		FadeIn();
		aud = _aud;
		vocalFX = vocalSFX;
		if (instant)
		{
			incrementalText.Clear();
			incrementalText.Append(goalText);
			text.Text = incrementalText.ToString();

		}
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
		anim.Stop();
		anim.Play("Select");
	}

	public void Unselect()
	{
		anim.Play("Unselect");
	}
}
