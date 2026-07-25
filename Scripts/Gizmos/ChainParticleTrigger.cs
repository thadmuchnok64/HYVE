using Godot;
using System;

public partial class ChainParticleTrigger : Node3D
{
	public void Launch()
	{
		foreach(Node3D n in GetChildren())
		{
			((GpuParticles3D)n).Emitting = true;
		}
	}
}
