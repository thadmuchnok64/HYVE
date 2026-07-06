using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PCStateMachine : Entity
{
	[Export] public CharacterBody3D cb;
	[Export] public Node3D camPoint;
	[Export] float camSensitivity = .5f;
	[Export] public AnimationTree anim;
	[Export] public Node3D meshRoot;
	[Export] public Weapon currentWeapon;
	[Export] float maxStamina;


	[Export] PCState startingState;
	[Export] public AudioStreamPlayer3D aud;
	[Export] bool debugState = false;
	[Export] Godot.Collections.Array<AudioStream> footsteps;
	[Export] float staminaRecoveryPerSec = 50f;
	[Export] float timeToRecoverStam = .5f;
	[Export] float postureRecoveryPerSec = 10f;
	[Export] float timeToRecoverPos = 1f;
	PCState currentState;

	public float stamina;
	float staminaTimer = 0;
	float postureTimer = 0;

	public bool ConsumeStamina(float cost)
	{
		if (stamina <= 0)
			return false;
		//stamina -= cost;
		stamina = Mathf.Clamp(stamina - cost, 0, maxStamina);
		HUDManager.instance.SetStamina(stamina, maxStamina);
		staminaTimer = 0;
		return true;
	}

	public void RecoverStamina(float val)
	{
		stamina = Mathf.Clamp(stamina + val, 0, maxStamina);
		HUDManager.instance.SetStamina(stamina, maxStamina);
	}

	public void ManageStamina(double delta, float recoveryPerSec)
	{
		if (staminaTimer < timeToRecoverStam)
			return;
		if(stamina < maxStamina)
		RecoverStamina((float)delta * recoveryPerSec);
	}

	public void RecoverPosture(float val)
	{
		posture = Mathf.Clamp(posture + val, 0, maxPosture);
		HUDManager.instance.SetPosture(posture, maxPosture);
	}

	public void ManagePosture(double delta, float recoveryPerSec)
	{
		if (postureTimer < timeToRecoverPos)
			return;
		if (posture < maxPosture)
			RecoverPosture((float)delta * recoveryPerSec);
	}

	public override void TakePostureDamage(float damage)
	{
		postureTimer = 0;
		base.TakePostureDamage(damage);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		stamina = maxStamina;
		health = maxHealth;
		posture = maxPosture;
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
		ChangeState(state);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		staminaTimer += (float)delta;
		postureTimer += (float)delta;
		ManageStamina(delta,staminaRecoveryPerSec);
		ManagePosture(delta,postureRecoveryPerSec);
		CamControl(delta);
		var newState = currentState.Process(delta);
		if (newState != null)
		{
			ChangeState(newState);
		}

	}

	private void CamControl(double delta) // expected to be called on process
	{
		Vector2 camDelta = new Vector2(Input.GetAxis("CamRight", "CamLeft"), Input.GetAxis("CamDown", "CamUp"));
		camPoint.RotateY(camDelta.X * camSensitivity * (float)delta);
		camPoint.RotateZ(camDelta.Y * camSensitivity * (float)delta);
		camPoint.Rotation = new Vector3(camPoint.Rotation.X, camPoint.Rotation.Y, Mathf.Clamp(camPoint.Rotation.Z, -30f, 30f));

	}

	public void EnableWeapon()
	{
		currentWeapon.SetWeaponActive(true);
	}

	public void DisableWeapon()
	{
		currentWeapon.SetWeaponActive(false);
	}

	public void HitByEnemy(float damage,Enemy enemyRef)
	{
		if (currentState is not PC_Block)
		{
			TakeDamage(damage);
			TakePostureDamage(damage);
		}
		ChangeState(currentState.HitByEnemyEvent());
		meshRoot.LookAt(GlobalPosition-(enemyRef.GlobalPosition - GlobalPosition),Vector3.Up);
		
		HUDManager.instance.SetHealth(health, maxHealth);
		HUDManager.instance.SetPosture(posture, maxPosture);

	}

	/*
	public string getAnimationName()
	{
		return currentState.animationName;
	}
	*/

	#region Anim Events

	public void Footstep()
	{
		aud.Stream = footsteps.ToList().GetRandomElement();
		aud.Play();
	}

	#endregion

}
