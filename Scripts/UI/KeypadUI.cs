using Godot;
using System;

public partial class KeypadUI : Control
{
	public string key = "1234";
	string currentCode = "";
	[Export] RichTextLabel codeText;
	[Export] AnimationPlayer anim;
	public Keypad keypad;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public bool TypeNumber(int num) // returns true if unlocked
	{
		currentCode = currentCode += num;
		codeText.Text = currentCode;
		if (currentCode.Length == key.Length)
		{
			if (currentCode.Equals(key))
			{
				CodePass();
				return true;
			}
			else
				CodeFailure();

			
		}
		return false;
	}

	private async void CodeFailure()
	{
		anim.Play("Error");
		keypad.FailSound();
		keypad.inputLocked = true;
		await ToSignal(GetTree().CreateTimer(.2f), SceneTreeTimer.SignalName.Timeout);
		ResetCode();
		await ToSignal(GetTree().CreateTimer(.2f), SceneTreeTimer.SignalName.Timeout);
		keypad.inputLocked = false;
	}

	private void CodePass()
	{
		anim.Play("Success");
		keypad.inputLocked = true;
		keypad.Unlock();

	}

	public void ResetCode()
	{
		currentCode = "";
		codeText.Text = "";
	}
}
