using Godot;
using System;

public partial class KeypadButton : MouseInteractable
{
	[Export] int keyPadKey = 1;
	[Export] bool isClearKey = false;
	[Export] AnimationPlayer anim;

	bool isMouseHovering = false;


	public override void MouseOn()
	{
		isMouseHovering = true;
	}

	public override void MouseOff()
	{
		isMouseHovering = false;
	}
	public override void MousePress()
	{
		anim.Play("Press");
		if (isClearKey)
			((Keypad)GetParent()).ClearCode();
		else
			((Keypad)GetParent()).SendInt(keyPadKey);
	}
}
