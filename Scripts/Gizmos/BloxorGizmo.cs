using Godot;
using System;

enum BloxorState { UPRIGHT, NORTHWARD, EASTWARD }

public partial class BloxorGizmo : InteractableObject
{
	[Export] Node3D cam;
	[Export] Node3D camPosition;
	[Export] BloxorState state = BloxorState.UPRIGHT;
	[Export] BloxorPC player;
	[Export] Node3D playerPivot;
	[Export] float baseYoffset = -11.152f;
	Vector3 targetPos,prevPos;
	Quaternion targetRot,prevRot;
	Vector3 targetPivotPos,prevPivotPos;
	[Export] AudioStream poundFX, impactFX,respawnFX, victoryFX;
	AudioStream nextAud;
	public AudioStreamPlayer3D aud;
	bool audReady = false;
	float timer = 1;
	[Export] float timeToTurn = .25f;
	

	bool active = false;
	bool falling = false;

	public override void _Ready()
	{
		base._Ready();
		targetPos = player.GlobalPosition;
		targetRot = player.GlobalBasis.GetRotationQuaternion();
		prevRot = targetRot;
		targetPivotPos = playerPivot.Position+new Vector3(0,baseYoffset,0);

		active = true; // have this called elsewhere in the future
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		timer += (float)delta;
		cam.GlobalPosition = camPosition.GlobalPosition;
		cam.GlobalRotation = camPosition.GlobalRotation;
		var lerpmod = Mathf.Clamp(timer/timeToTurn, 0, 1);
		if (!falling)
		{
			player.GlobalPosition = (prevPos.ReplaceY(prevPivotPos.Y)).Lerp((targetPos.ReplaceY(targetPivotPos.Y)), lerpmod);
			player.GlobalRotation = prevRot.Normalized().Slerp(targetRot.Normalized(), lerpmod).GetEuler();
			//playerPivot.Position = prevPivotPos.Lerp(targetPivotPos, lerpmod);
		}
		if ((lerpmod>=1))
		{
			if (audReady)
			{
				aud.Stream = nextAud;
				aud.Play();
				audReady = false;
				falling = !player.CheckIfSpotIsValid();
				if (falling)
				{
					Respawn();
				}
				else
				{
					if(player.CheckIfWon())
					{
						falling = true;
						aud.Stream = victoryFX;
						aud.Play();
					}
				}
			}
		}

	}

    private async void Respawn()
    {
        await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
		// Code to execute after 2 seconds
		aud.Stream = respawnFX;
		falling = false;
		state = BloxorState.UPRIGHT;
		aud.Play();
		player.Respawn();
        targetPos = player.GlobalPosition;
        targetRot = player.GlobalBasis.GetRotationQuaternion();
        prevRot = targetRot;
        targetPivotPos = playerPivot.Position + new Vector3(0, baseYoffset, 0);
    }

    public override bool ManageInput(InputEvent @event)
	{
		if (falling)
			return false;
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
		var currentRot = Quaternion.FromEuler(player.GlobalRotation);
		switch (state)
		{
			case BloxorState.UPRIGHT:
				targetPos = player.GlobalPosition - new Vector3(0,0,sign*1.5f);
				//targetRot = Quaternion.FromEuler(playerPivot.GlobalRotation + new Vector3(sign*MathF.PI/2f, 0, 0));
				targetPivotPos = Vector3.Zero.ReplaceY(baseYoffset);
				nextAud = poundFX;
				state = BloxorState.NORTHWARD;
				break;
			case BloxorState.NORTHWARD:
				targetPos = player.GlobalPosition - new Vector3(0, 0, sign * 1.5f);
				targetPivotPos = new Vector3(0,.5f+baseYoffset,0);
				nextAud = impactFX;
				state = BloxorState.UPRIGHT;
				break;
			case BloxorState.EASTWARD:
				targetPos = player.GlobalPosition - new Vector3(0, 0, sign * 1f);
				targetPivotPos = Vector3.Zero.ReplaceY(baseYoffset);
				nextAud = impactFX;
				break;
		}
		targetRot = currentRot = new Quaternion(Vector3.Right, -sign * MathF.PI / 2f) * currentRot;


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
		var currentRot = Quaternion.FromEuler(player.GlobalRotation);
		switch (state)
		{
			case BloxorState.UPRIGHT:
				targetPos = player.GlobalPosition - new Vector3(1.5f*sign, 0, 0);
				targetPivotPos = Vector3.Zero.ReplaceY(baseYoffset);
				nextAud = poundFX;
				state = BloxorState.EASTWARD;
				break;
			case BloxorState.NORTHWARD:
				targetPos = player.GlobalPosition - new Vector3(1f*sign, 0, 0);
				nextAud = impactFX;
				targetPivotPos = Vector3.Zero.ReplaceY(baseYoffset);
				break;
			case BloxorState.EASTWARD:
				targetPos = player.GlobalPosition - new Vector3(1.5f*sign, 0, 0);
				targetPivotPos = new Vector3(0, .5f+baseYoffset, 0);
				nextAud = impactFX;
				state = BloxorState.UPRIGHT;
				break;
		}
		targetRot = currentRot = new Quaternion(Vector3.Forward, -sign * MathF.PI / 2f) * currentRot;


	}

}
