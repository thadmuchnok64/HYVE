using Godot;
using System;

public partial class DialogueNode : GraphNode
{

	public void Destruct()
	{
		QueueFree();
	}

	public void SetSize(Vector2 newSize)
	{
		Size = newSize;
	}
}
