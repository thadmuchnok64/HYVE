using Godot;
using System;

public partial class DialogueState : UIState
{

	public override UIState Enter()
	{
		base.Enter();
		return this;
	}

	public override UIState Exit()
	{
		return base.Exit();
	}

	public override UIState NavForward()
	{
		return null;
	}

}
