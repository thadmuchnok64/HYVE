using Godot;
using System;

public partial class EquipmentScreen : UIState
{
	[Export] GridContainer inventoryBacklog;
	[Export] InventorySlot equippedWeaponSlot;
	InventoryManager currentInventory;

    public override UIState Enter()
    {
        base.Enter();
		((InventorySlot)inventoryBacklog.GetChild(navIndex1D)).Highlight();
		return this;
    }

    public override UIState Exit()
    {
		foreach(InventorySlot slot in inventoryBacklog.GetChildren())
		{
			slot.Unhighlight();
		}
        return base.Exit();
    }
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

    public override UIState NavForward()
    {
		var slot = ((InventorySlot)inventoryBacklog.GetChildren()[navIndex1D]);
		if (slot == null)
			return null;
		equippedWeaponSlot.Populate(slot.GetCollectable());
		return null;
    }


}
