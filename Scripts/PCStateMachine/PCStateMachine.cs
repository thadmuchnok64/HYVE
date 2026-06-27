using Godot;
using System;

public partial class PCStateMachine : Node3D
{
	[Export] public CharacterBody3D cb;
	[Export] public Node3D camPoint;
	[Export] public AnimationTree anim;
	[Export] public Node3D meshRoot;
	[Export] public Weapon currentWeapon;


	[Export] PCState startingState;
	[Export] AudioStreamPlayer3D aud;
	[Export] bool debugState = false;
	PCState currentState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (PCState state in GetChildren())
		{
			//state.camPoint = camPoint;
			//state.SetAnimationTree(anim);
			//state.SetAudioSource(aud);
		}
		ChangeState(startingState);

	}

	public void ChangeState(PCState state)
	{
		if (currentState != null)
		{
			currentState.Exit();
			currentState.ExitAnimation();
		}
		currentState = state;
		currentState.Enter();
		currentState.EntryAnimation();
		if (debugState)
			GD.Print(currentState.Name);
	}

	public override void _PhysicsProcess(double delta)
	{
		var newState = currentState.PhysicsProcess(delta);
		if (newState != null)
		{
			ChangeState(newState);
		}

	}

	public override void _Input(InputEvent @event)
	{
		PCState state = currentState.ManageInput(@event);
		if(state!=null)
		ChangeState(currentState.ManageInput(@event));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var newState = currentState.Process(delta);
		if (newState != null)
		{
			ChangeState(newState);
		}

	}

	public void EnableWeapon()
	{
		currentWeapon.SetWeaponActive(true);
	}

	public void DisableWeapon()
	{
		currentWeapon.SetWeaponActive(false);

	}

	/*
	public string getAnimationName()
	{
		return currentState.animationName;
	}
	*/

}
