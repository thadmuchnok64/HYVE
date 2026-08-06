using Godot;
using System;

public partial class KeypadButton : Node3D
{
	[Export] int keyPadKey = 1;
	bool isMouseHovering = false;


	public override void _Input(InputEvent @event)
	{
		if (!isMouseHovering)
			return;
		if(@event is InputEventMouseButton)
		{
			if(((InputEventMouseButton)@event).ButtonIndex == MouseButton.Left)
			{
				ClickKey();
			}
		}
	}
	public void MouseOn()
	{
		GD.Print("poop");
		isMouseHovering = true;
	}

	public void MouseOff()
	{
		isMouseHovering = false;
	}
	public void ClickKey()
	{
		GD.Print(keyPadKey);
	}
}
