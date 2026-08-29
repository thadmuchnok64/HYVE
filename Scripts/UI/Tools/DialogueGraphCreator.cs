using Godot;
using System;
using System.Linq;

public partial class DialogueGraphCreator : Control
{
	[Export] PackedScene dialogueNode;
	[Export] PackedScene playerNode;

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

	public void NewResponse()
	{
		var newNode = playerNode.Instantiate();
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


	}

	public DA_DialogueNode ParseNode(DialogueNode d)
	{
		if (d is DialogueNPCStatement)
		{

			DA_DialogueNPCStatement statement = new DA_DialogueNPCStatement();
			//statement.text = d.
			statement.text = ((DialogueNPCStatement)d).responseText.Text;
			var connections = graphEdit.GetConnectionListFromNode(d.Name);
			if (connections.Count > 0)
			{
				foreach (var c in connections)
				{
					string name = (StringName)c["to_node"];
					if (name != d.Name)
					{
						var child = (DialogueNode)graphEdit.GetChildren().Where(n => n is DialogueNode && n.Name == name).First();
						statement.nextNode = ParseNode(child);
					}
				}
			}

			return statement;

		}
		else if (d is DialoguePlayerStatement)
		{
			DA_DialogueBranch statement = new DA_DialogueBranch();
			var connections = graphEdit.GetConnectionListFromNode(d.Name);
			if (connections.Count > 0)
			{
				int i = ((DialoguePlayerStatement)d).responses.Count -1;
				foreach (var c in connections)
				{
					string name = (StringName)c["to_node"];
					if (name != d.Name)
					{
						DA_DialogueResponse response = new DA_DialogueResponse();
						response.text = ((DialoguePlayerStatement)d).responses[i].Text;
						var child = (DialogueNode)graphEdit.GetChildren().Where(n => n is DialogueNode && n.Name == name).First();
						response.nextNode = ParseNode(child);
						statement.responses.Add(response);
						i--;
					}
				}
			}
			return statement;
		}

			return null;


	}

}


