using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InventoryManager : Node3D
{

	public List<Collectable> items;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		items = new List<Collectable>();
			//load stuff here;
	}

	
	public void InsertCollectable(Collectable item)
	{
		if(items.Contains(item)){
			GD.Print("already have this, ya dingus");
			return; // in the future, check for stack size
		}
		items.Add(item);
		GD.Print($"added {item.DisplayName}");
	}

	public bool DoesHaveItem(string itemName)
	{
		return items.Select(i => i.DisplayName == itemName).Count() > 0;
	}
}
