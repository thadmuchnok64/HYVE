using Godot;
using System;

public partial class EquipmentScreen : Control
{
	[Export] GridContainer inventoryBacklog;
	InventoryManager currentInventory;
	public void RefreshInventory(InventoryManager inventory)
	{
		currentInventory = inventory;
		foreach(InventorySlot slot in inventoryBacklog.GetChildren())
		{
			slot.Visible = false;
		}
		int itr = 0;
		foreach( Collectable c in currentInventory.items)
		{
			((InventorySlot)inventoryBacklog.GetChild(itr)).Populate(c);
			((InventorySlot)inventoryBacklog.GetChild(itr)).Visible = true;

			itr++;
		}
	}
}
