using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class PCStateMachine : Entity
{
	[Export] public CharacterBody3D cb;
	[Export] public Node3D camPoint, camPivot;
	[Export] float camSensitivity = .5f;
	[Export] public AnimationTree anim;
	[Export] public Node3D meshRoot;
	[Export] public Weapon currentWeapon;
	[Export] public InventoryManager inventory;
	[Export] float maxStamina;
	[Export] RayCast3D interactionRay;
	[Export] Area3D lockOnArea;

	[Export] PCState startingState;
	[Export] public AudioStreamPlayer3D aud;
	[Export] bool debugState = false;
	[Export] Godot.Collections.Array<AudioStream> footsteps;
    [Export] Godot.Collections.Array<AudioStream> sfx;

    [Export] float staminaRecoveryPerSec = 50f;
	[Export] float timeToRecoverStam = .5f;
	[Export] float postureRecoveryPerSec = 10f;
	[Export] float timeToRecoverPos = 1f;

    [Export] CameraController cam;
	[Export] Node3D mainCamPoint, inventoryCamPoint;
	[Export] public Node3D trackingPoint;


	[Export] Godot.Collections.Array<GpuParticles3D> slidingParticles;
	PCState currentState;

	public float stamina;
	float staminaTimer = 0;
	float postureTimer = 0;
	public Node3D trackingObject;
	public bool tracking = false;


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
		cam.SetTrackingObject(mainCamPoint);

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
		if (state == null)
			return;
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
        if (state!=null)
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
		InteractUI();
		if (tracking)
		{
			if (!trackingObject.GetNode<Enemy>("Enemy").alive)
			{
				AttemptTracking();
			}
			HUDManager.instance.SnapTrackerToPoint(trackingObject.GetNode<Enemy>("Enemy").trackingPoint.GlobalPosition, GameMaster.Instance.mainCamRef);
		}
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
		camPivot.RotateZ(camDelta.Y * camSensitivity * (float)delta);
		camPivot.Rotation = new Vector3(camPivot.Rotation.X, camPivot.Rotation.Y, Mathf.Clamp(camPivot.Rotation.Z, -30f, 30f));

	}

	private void InteractUI()
	{
		if (CanInteract())
		{
			HUDManager.instance.ToggleInteractText(true, ((GizmoTrigger)interactionRay.GetCollider()).interactText);
		}
		else
		{
			HUDManager.instance.ToggleInteractText(false);
		}
	}

	public bool CanInteract()
	{
		if (interactionRay.IsColliding())
		{
			if (interactionRay.GetCollider() is GizmoTrigger)
			{
				return true;
			}
		}
		return false;
	}

	public void RotateMesh(Vector3 newRot) // global space
	{
		meshRoot.GlobalRotation = newRot;
		meshRoot.Rotation = new Vector3(0, meshRoot.Rotation.Y, 0);
	}

	public InteractableObject TryInteraction()
	{
		if (CanInteract()) {
			var giz = (GizmoTrigger)interactionRay.GetCollider();
			giz.TryTriggerGizmo(this);
			return giz.interactable;
		}
		return null;
	}

	public void ForceUninteract()
	{
		ChangeState(currentState.BreakInteractEvent());
	}

	public void EnableWeapon()
	{
        currentWeapon.SetAttackType(AttackType.LIGHT);
        currentWeapon.SetWeaponActive(true);
	}

	public void EnableWeaponHeavyAttack()
	{
		currentWeapon.SetAttackType(AttackType.HEAVY);
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

	public Node3D AttemptTracking()
	{
		if (tracking)
		{
			tracking = false;
			HUDManager.instance.ShowTracker(tracking);
			return null;
		}
		var potentialBodies = lockOnArea.GetOverlappingBodies().ToList();
		var bodies = potentialBodies.Where(e => ((Enemy)e.GetChild(0)).alive).ToList();
		if (bodies.Count <= 0)
			return null;
		tracking = true;
		HUDManager.instance.ShowTracker(tracking);
		trackingObject = potentialBodies.MinBy(b => lockOnArea.GlobalPosition.DistanceTo(b.GlobalPosition));
		return trackingObject;
	}

	public void Slide()
	{
		PlaySound(0); // slide fx
		foreach(GpuParticles3D part in slidingParticles)
		{
			part.Emitting = true;
		}
	}

	public bool TriggerInventory()
	{
        bool open = HUDManager.instance.ToggleInventory(inventory);
        if (open)
        {
            cam.SetTrackingObject(inventoryCamPoint);
        }
        else
        {
            cam.SetTrackingObject(mainCamPoint);
        }
		return open;
    }

	public void SetDefaultCamPoint()
	{
		cam.SetTrackingObject(mainCamPoint);
	}

	/*
	public string getAnimationName()
	{
		return currentState.animationName;
	}
	*/

	public void PlaySound(int index)
	{
        SoundManager.Instance.RequesetSFXSoundAtLocation(sfx[index], GlobalPosition);
	}

	#region Anim Events

	public void Footstep()
	{
		aud.Stream = footsteps.ToList().GetRandomElement();
		aud.Play();
	}

	#endregion

}
