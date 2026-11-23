using Godot;
using System;

public partial class PC_ItemCollect : PCState
{
	[Export] PCState idleState;
	[Export] Node3D camPoint;
	[Export] MeshInstance3D itemDisplayMesh;
	[Export] AudioStream collectSound;
	[Export] float rotateSpeed = MathF.PI/2f;
	Collectable storedCollectable;

	public override PCState ManageInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Inventory"))
		{
				return idleState;
		}
		if (@event.IsActionPressed("Interact"))
		{
			return idleState;
		}


		return base.ManageInput(@event);
	}

	public override PCState Enter()
	{
		base.Enter();
		stateMachine.cam.SetTrackingObject(camPoint);
		stateMachine.SetWeaponVisibility(false);
		SoundManager.Instance.RequesetSFXSoundAtLocation(collectSound, itemDisplayMesh.GlobalPosition);
		itemDisplayMesh.RotationDegrees = Vector3.Zero;
		return null;
	}
	public override void EntryAnimation()
	{
		base.EntryAnimation();
		anim.Set("parameters/interact/Transition/transition_request", animMeta);


	}

	public override PCState Exit()
	{
		stateMachine.SetDefaultCamPoint();
		stateMachine.SetWeaponVisibility(true);
		itemDisplayMesh.Mesh = null;
		HUDManager.instance.ItemCollectOut();

		return null;
	}

	public void PopulateItem(Collectable item)
	{
		itemDisplayMesh.Mesh = item.displayMesh;
		storedCollectable = item; // storing this incase we need it;
		HUDManager.instance.ItemCollectIn(item);

	}

	public override PCState Process(double delta)
	{
		Vector2 movement = new Vector2(Input.GetAxis("MoveRight", "MoveLeft"), Input.GetAxis("MoveDown", "MoveUp"));
		itemDisplayMesh.GlobalRotate(Vector3.Up, rotateSpeed * movement.X * (float)delta);
		itemDisplayMesh.GlobalRotate(Vector3.Right, rotateSpeed * movement.Y * (float)delta);


		return base.Process(delta);
	}
}
