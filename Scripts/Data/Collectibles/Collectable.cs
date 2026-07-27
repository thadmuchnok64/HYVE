using Godot;
using System;

[GlobalClass] public partial class Collectable : Resource
{
	[Export] public string DisplayName = "Health Pack";
	[Export] public int itemCode;
	[Export] public int maxHeldAmount = 1;
	[Export] public Texture2D inventoryIcon;
	// How many the player can collect at one time. can be left at 1 if the object is unique, or higher for say health packs
}
