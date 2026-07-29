using Godot;
using System;

public partial class InventorySlot : NavigableUIElement
{
	[Export] TextureRect icon;
	Collectable storedCollectable;
	public void Populate(Collectable collectable)
	{
		icon.Texture = collectable.inventoryIcon;
		storedCollectable = collectable;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Collectable GetCollectable()
	{
		return storedCollectable;
	}
}
