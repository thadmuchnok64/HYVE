using Godot;
using System;
using System.Linq;

public partial class Enemy : Entity
{
	[Export] public NavigationAgent3D nav;
	[Export] public CharacterBody3D cb;
	[Export] public Node3D meshRoot;
	[Export] public AnimationTree anim;
	[Export] EnemyState startingState;
	[Export] Area3D detectionSphere;
	[Export] protected MeshInstance3D meshInstance;
	[Export] Mesh headlessMesh;
    [Export] protected PackedScene bloodSplat;
    [Export] protected PackedScene bloodSplatSmall;
	[Export] Godot.Collections.Array<AudioStream> sfx;

    [Export] protected Node3D bloodPoint;
	[Export] public Node3D trackingPoint;
	[Export] public bool agrod = false;
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
		base._Ready();
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
			agrod = true;
			SwitchState(currentState.DetectPlayerEvent());
		}

	}

	public void HitEnemyFromDirection(float damage, SwingDirection dir)
	{
		TakeDamage(damage);
		TakePostureDamage(damage);

        var inst = bloodSplatSmall.Instantiate();
        cb.AddSibling(inst);
        ((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;

        var state = currentState.HitEvent();
        if (state != null)
		{
			SwitchState(state);
		}
	}

	public override void Die()
	{
		base.Die();
        var inst = bloodSplat.Instantiate();
        cb.AddSibling(inst);
        ((Node3D)inst).GlobalPosition = bloodPoint.GlobalPosition;
        meshInstance.Mesh = headlessMesh;

    }

    public void PlaySound(int soundIndex)
	{
		SoundManager.Instance.RequesetSFXSoundAtLocation(sfx[soundIndex], GlobalPosition);
	}


}
