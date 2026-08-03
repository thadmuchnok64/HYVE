using Godot;
using System;

public partial class BloxorGizmo : InteractableObject
{
	[Export] Node3D cam;
	[Export] Node3D camPosition;
	[Export] BloxorState state = BloxorState.UPRIGHT;
	[Export] Node3D player, playerPivot;
	Vector3 targetPos,prevPos;
	Vector3 targetRot,prevRot;
	Vector3 targetPivotPos,prevPivotPos;
	[Export] AudioStream poundFX, impactFX;
	AudioStream nextAud;
	public AudioStreamPlayer3D aud;
	bool audReady = false;
	float timer = 1;
	[Export] float timeToTurn = .25f;
	

	enum BloxorState { UPRIGHT, NORTHWARD, EASTWARD}
	bool active = false;

	public override void _Ready()
	{
		base._Ready();
		targetPos = player.Position;
		targetRot = playerPivot.Rotation;
		targetPivotPos = playerPivot.Position;

		active = true; // have this called elsewhere in the future
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		timer += (float)delta;
		cam.GlobalPosition = camPosition.GlobalPosition;
		cam.GlobalRotation = camPosition.GlobalRotation;
		var lerpmod = Mathf.Clamp(timer/timeToTurn, 0, 1);
		if ((lerpmod>=1))
		{
			if (audReady)
			{
				aud.Stream = nextAud;
				aud.Play();
				audReady = false;
			}
		}
		player.Position = prevPos.Lerp(targetPos,lerpmod);
		playerPivot.Rotation = prevRot.Lerp(targetRot,lerpmod);
		playerPivot.Position = prevPivotPos.Lerp(targetPivotPos,lerpmod);
	}

	public override bool ManageInput(InputEvent @event)
	{
		Vector2 movement = new Vector2(Input.GetAxis("MoveLeft", "MoveRight"), Input.GetAxis("MoveUp", "MoveDown"));

		if (@event.IsActionPressed("MoveLeft"))
		{
			MoveHorz(-1);
		}
		if (@event.IsActionPressed("MoveRight"))
		{
			MoveHorz(1);
		}
		if (@event.IsActionPressed("MoveUp"))
		{
			MoveVert(-1);
		}
		if (@event.IsActionPressed("MoveDown"))
		{
			MoveVert(1);
		}
		return base.ManageInput(@event);
	}

	public void MoveVert(int sign) {

		if (!CanMove())
			return;
		prevPivotPos = targetPivotPos;
		prevPos = targetPos;
		prevRot = targetRot;
		timer = 0;
		audReady = true;
		switch (state)
		{
			case BloxorState.UPRIGHT:
				targetPos = player.Position + new Vector3(0,0,sign*1.5f);
				targetRot = playerPivot.Rotation + new Vector3(sign*MathF.PI/2f, 0, 0);
				targetPivotPos = Vector3.Zero;
				nextAud = poundFX;
				state = BloxorState.NORTHWARD;
				break;
			case BloxorState.NORTHWARD:
				targetPos = player.Position + new Vector3(0, 0, sign * 1.5f);
				targetRot = playerPivot.Rotation + new Vector3(sign * MathF.PI / 2f, 0, 0);
				targetPivotPos = new Vector3(0,.5f,0);
				nextAud = impactFX;
				state = BloxorState.UPRIGHT;
				break;
			case BloxorState.EASTWARD:
				targetPos = player.Position + new Vector3(0, 0, sign * 1f);
				targetRot = playerPivot.Rotation + new Vector3(0, sign * MathF.PI / 2f,0);
				targetPivotPos = Vector3.Zero;
				nextAud = impactFX;

				break;
		}
	}
	
	private bool CanMove()
	{
		return (timer / timeToTurn) > 1;
	}
	public void MoveHorz(int sign)
	{
		if (!CanMove())
			return;
		audReady = true;
		prevPivotPos = targetPivotPos;
		prevPos = targetPos;
		prevRot = targetRot;
		timer = 0;
		switch (state)
		{
			case BloxorState.UPRIGHT:
				targetPos = player.Position + new Vector3(1.5f*sign, 0, 0);
				targetRot = playerPivot.Rotation + new Vector3(0,0,-sign * MathF.PI / 2f);
				targetPivotPos = Vector3.Zero;
				nextAud = poundFX;
				state = BloxorState.EASTWARD;
				break;
			case BloxorState.NORTHWARD:
				targetPos = player.Position + new Vector3(1f*sign, 0, 0);
				targetRot = playerPivot.Rotation + new Vector3(0,sign * MathF.PI / 2f, 0);
				nextAud = impactFX;
				targetPivotPos = Vector3.Zero;
				break;
			case BloxorState.EASTWARD:
				targetPos = player.Position + new Vector3(1.5f*sign, 0, 0);
				targetRot = playerPivot.Rotation + new Vector3(0, 0, -sign * MathF.PI / 2f);
				targetPivotPos = new Vector3(0, .5f, 0);
				nextAud = impactFX;
				state = BloxorState.UPRIGHT;

				break;
		}
	}

}
