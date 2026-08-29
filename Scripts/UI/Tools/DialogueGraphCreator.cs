using Godot;
using System;

public partial class DialogueGraphCreator : Control
{
	[Export] PackedScene dialogueNode;
	[Export] GraphEdit graphEdit;
	[Export] TextEdit saveText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void NewNodeButton()
	{
		var newNode = dialogueNode.Instantiate();
		graphEdit.AddChild(newNode);
	}

	public void ConnectionRequest(StringName from, int fromPort, StringName to, int toPort)
	{
		graphEdit.ConnectNode(from, fromPort, to, toPort);
	}

	public void DisconnectRequest(StringName from, int fromPort, StringName to, int toPort)
	{
		graphEdit.DisconnectNode(from, fromPort, to, toPort);
	}

	public void SaveDialogueTree()
	{
		var dia = new DA_DialogueTree();
		DialogueNode d;
		bool found = false;
		foreach(Node n in graphEdit.GetChildren())
		{
			//C:\Users\Thad\Desktop
			if (n is DialogueNode)
			{
				d = (DialogueNode)n;
				if (!found)
				{
					dia.startingNode = ParseNode(d);
					ResourceSaver.Save(dia, $"res://Scripts/Data/Dialogue/Trees/{saveText.Text}.tres"); // try user instead of res? idk
				}
				found = true;

			}
		}

		/*
		var scene = new PackedScene();
		foreach(Node n in graphEdit.GetChildren())
		{
			if(n is DialogueNode)
			{
				n.Owner = graphEdit;
				foreach(Node m in n.GetChildren())
				{
					m.Owner = graphEdit;
					m.SceneFilePath = ""; // blocks duplicate childs
				}
			}
		}
		var res = scene.Pack(graphEdit);
		if(res == Error.Ok)
		{
			ResourceSaver.Save(scene, $"res://Scripts/Data/Dialogue/{saveText.Text}.scn");
		}
		*/
	}

	public DA_DialogueNode ParseNode(DialogueNode d)
	{
		GD.Print(d.Name);
		if (d is DialogueNPCStatement)
		{

			DA_DialogueNPCStatement statement = new DA_DialogueNPCStatement();
			//statement.text = d.
			statement.text = ((DialogueNPCStatement)d).responseText.Text;
			var connections = graphEdit.GetConnectionListFromNode(d.Name);
			if (connections.Count > 0) {
				GD.Print("fuk");
				GD.Print((StringName)connections[0]["to_node"]);
				statement.nextNode = ParseNode((DialogueNode)d.GetParent().FindChild((StringName)connections[0]["to_node"]));
					}

			return statement;

		}

		return null;


	}

}


