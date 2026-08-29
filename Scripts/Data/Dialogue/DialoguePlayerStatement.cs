using Godot;
using System;
using System.Collections.Generic;

public partial class DialoguePlayerStatement : DialogueNode
{
	[Export] PackedScene response;
	public List<TextEdit> responses;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		responses = new List<TextEdit>();
		responses.Add((TextEdit)FindChild("TextEdit"));
	}

	public void AddResponse()
	{
		if (responses.Count > 3)
		{
			GD.Print("Cant go higher than 4 responses");
			return;
		}
		var r = response.Instantiate();
		responses.Add((TextEdit)r);
		AddChild(r);
		SetSlotEnabledRight(responses.Count -1,true);
	}

	public void RemoveResponse()
	{
		if (responses.Count < 2)
		{
			GD.Print("Cant go lower than 1 response");
			return;
		}
		SetSlotEnabledRight(responses.Count - 1, false);
		responses[responses.Count - 1].QueueFree();
		responses.RemoveAt(responses.Count - 1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
