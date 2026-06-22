using Godot;
using System;
using System.Linq;

public partial class Enemy : Entity
{
	[Export] public NavigationAgent3D nav;
	[Export] public CharacterBody3D cb;
	[Export] public Node3D meshRoot;
	[Export] public AnimationTree anim;
	public Node3D player;
	[Export] EnemyState startingState;
	[Export] Area3D detectionSphere;
	protected EnemyState currentState;
	// Called when the node enters the scene tree for the first time
	// 

	public void SwitchState(EnemyState state)
	{
		if (state == null)
			return;

		if (currentState != null)
		{
			currentState.Exit();
		}
		state.Enter(this);

		currentState = state;
	}
	public override void _Ready()
	{
		SwitchState(startingState);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		FindDetectedObjects();
		SwitchState(currentState.Process(delta));
	}

	public override void _PhysicsProcess(double delta)
	{
		SwitchState(currentState.PhysicsProcess(delta));
	}

	public void FindDetectedObjects()
	{
		if (detectionSphere.HasOverlappingBodies())
		{
			player = detectionSphere.GetOverlappingBodies()[0];
			SwitchState(currentState.DetectPlayerEvent());
		}

	}


}
