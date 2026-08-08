using Godot;
using System;

public partial class CameraController : Camera3D
{

	[Export] Node3D currentTrackingObject;
	[Export] Curve switchPositionCurve;
	[Export] float timeToSwitchPos = .3f;
	[Export] Curve cameraCurve;
	[Export] Node3D mouseRay;
	MouseInteractable selectedMouseObject = null;
	float timer = 5;
	Vector3 prevPosition = Vector3.Zero;
	Quaternion prevRot;

	bool mouseInteracting = false;
	public void SetTrackingObject(Node3D newObj) // might change this later to use camera states instead.
	{
		prevPosition = GlobalPosition;
		prevRot = GlobalBasis.GetRotationQuaternion();
		currentTrackingObject = newObj;
		timer = 0;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += (float)delta;
		var val = switchPositionCurve.Sample(Math.Clamp(timer / timeToSwitchPos, 0, 1));

        GlobalPosition = prevPosition.Lerp(currentTrackingObject.GlobalPosition, val);
		GlobalRotation = prevRot.Slerp(currentTrackingObject.GlobalBasis.GetRotationQuaternion(),cameraCurve.Sample(val)).GetEuler();
		//Quaternion = prevRot.Slerp(currentTrackingObject.Basis.GetRotationQuaternion(), val);
		mouseRay.GlobalPosition = ProjectPosition(GetViewport().GetMousePosition(),.1f);
		mouseRay.LookAt(ProjectPosition(GetViewport().GetMousePosition(), 1f));
		MouseUpdate();
		base._Process(delta);
    }

	private void MouseUpdate()
	{
		if (mouseInteracting)
		{
			var col = ((RayCast3D)mouseRay.GetChild(0)).GetCollider();
			if (col != null && col is MouseInteractable)
			{
				if (selectedMouseObject == null || selectedMouseObject != col)
				{
						if (selectedMouseObject != null)
							selectedMouseObject.MouseOff();
					selectedMouseObject = ((MouseInteractable)col);
					selectedMouseObject.MouseOn();
				}
			}
			else
			{
				if (selectedMouseObject != null)
				{
					selectedMouseObject.MouseOff();
					selectedMouseObject = null;
				}
			}
		}
	}
	public void TriggerMouseInteraction(bool interacting)
	{
		mouseInteracting = interacting;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			if (((InputEventMouseButton)@event).ButtonIndex == MouseButton.Left && @event.IsReleased())
			{
				MousePress();
			}
		}
	}

	private void MousePress()
	{
		if (selectedMouseObject != null)
			selectedMouseObject.MousePress();
	}
}
