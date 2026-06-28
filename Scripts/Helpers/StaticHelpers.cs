using Godot;
using System;
using System.Collections.Generic;

public static class StaticHelpers
{

	public static List<T> GetChildrenOfType<T>(Node3D root, bool recursive = false) where T : Node3D
	{
		var result = new List<T>();

		foreach (Node3D child in root.GetChildren())
		{
			if (child is T typedChild)
				result.Add(typedChild);

			if (recursive)
				result.AddRange(GetChildrenOfTypeRecursive<T>(child));
		}

		return result;
	}


	// Helper for recursion
	private static List<T> GetChildrenOfTypeRecursive<T>(Node root) where T : Node
	{
		var result = new List<T>();

		foreach (Node child in root.GetChildren())
		{
			if (child is T typedChild)
				result.Add(typedChild);

			result.AddRange(GetChildrenOfTypeRecursive<T>(child));
		}

		return result;
	}

	public static T GetRandomElement<T>(this List<T> list)
	{
		Random rand = new Random();
		return list[rand.Next(list.Count)];
	}

	public static Variant GetRandomElement<variant>(this Godot.Collections.Array<Variant> list)
	{
		Random rand = new Random();
		return list[rand.Next(list.Count)];
	}

}
