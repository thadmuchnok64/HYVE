using Godot;
using System;
using System.Linq;

public partial class MonkeyBars : InteractableObject
{
	[Export] Node3D playerWarpPosition;
	[Export] Node3D barPivotParent;
	[Export] float playerYOffset = -1.1F;
	bool inUse = false;
	PCStateMachine pc;
	Node3D currentPivot;
	bool wantsToMove = false;
	bool wantsToMoveForward;
	public override void TriggerGizmo(PCStateMachine pc)
	{
		inUse = true;
		interactSuccess = true;
		this.pc = pc;
		currentPivot = (Node3D)barPivotParent.GetChildren().MinBy(n => ((Node3D)n).GlobalPosition.DistanceTo(pc.GlobalPosition));

		pc.cb.GlobalPosition = currentPivot.GlobalPosition + new Vector3(0,playerYOffset,0);
		pc.RotateMesh(currentPivot.GlobalRotation);
		//}
	}
	public override void _Process(double delta)
	{
		if (inUse)
		{
			if (wantsToMove)
			{
				ManageMovement((float)delta);
				return;

			}

			Vector2 movement = new Vector2(Input.GetAxis("MoveLeft", "MoveRight"), Input.GetAxis("MoveUp", "MoveDown"));
			if (movement.Length() <= .05f)
			{
				wantsToMove = false;
				pc.anim.Set("parameters/interact/MonkeyBar/transition_request", "idle");

				return;
			}
			wantsToMove = true;
			var force = movement.Normalized();
			var addedForce = new Vector3(force.X, 0, force.Y);
			addedForce = addedForce.Rotated(new Vector3(0, 1, 0),GameMaster.Instance.mainCamRef.GlobalRotation.Y);
			var dot = addedForce.Normalized().Dot((pc.meshRoot.GlobalBasis.Z * new Vector3(1, 0, 1)).Normalized());
			if (dot < 0)
			{
				if (pc.animationDelta > 0)
				{
					wantsToMove = false;
					return;
				}
				if (!(barPivotParent.GetChildren().IndexOf(currentPivot) > 0))
				{
					wantsToMove = false;
					pc.anim.Set("parameters/interact/MonkeyBar/transition_request", "idle");
					return;

				}


				wantsToMoveForward = false;
				pc.anim.Set("parameters/interact/MonkeyBar/transition_request", "backward");
				//GD.Print("BACKWARD");
			}
			else if (dot > 0)
			{
				if (pc.animationDelta > 0)
				{
					wantsToMove = false;
					return;
				}
				if (!(barPivotParent.GetChildren().IndexOf(currentPivot) < barPivotParent.GetChildCount() - 1))
				{
					wantsToMove = false;
					pc.anim.Set("parameters/interact/MonkeyBar/transition_request", "idle");
					return;

				}

				wantsToMoveForward = true;
				pc.anim.Set("parameters/interact/MonkeyBar/transition_request", "forward");

				//GD.Print("FORWARD");
			}
			//pc.ForceUninteract();
			//used = false;
		}
	}

	public void ManageMovement(float delta)
	{
		if (inUse)
		{
			if (wantsToMove)
			{
				if (wantsToMoveForward)
				{
					//currentPivot = (Node3D)barPivotParent.GetChildren().MinBy(n => ((Node3D)n).GlobalPosition.DistanceTo(pc.GlobalPosition));
					if (barPivotParent.GetChildren().IndexOf(currentPivot) < barPivotParent.GetChildCount() - 1)
					{
						pc.cb.GlobalPosition = currentPivot.GlobalPosition.Lerp(((Node3D)barPivotParent.GetChild(barPivotParent.GetChildren().IndexOf(currentPivot) + 1)).GlobalPosition,pc.animationDelta.Clamp01()) + new Vector3(0, playerYOffset, 0);
						pc.RotateMesh(currentPivot.GlobalRotation);
						if (pc.animationDelta >= .95f)
						{
							wantsToMove = false;
							currentPivot = (Node3D)barPivotParent.GetChild(barPivotParent.GetChildren().IndexOf(currentPivot) + 1);
							pc.cb.GlobalPosition = currentPivot.GlobalPosition + new Vector3(0, playerYOffset, 0);

						}
					}
				}
				else
				{
					if (barPivotParent.GetChildren().IndexOf(currentPivot) >0)
					{
						pc.cb.GlobalPosition = currentPivot.GlobalPosition.Lerp(((Node3D)barPivotParent.GetChild(barPivotParent.GetChildren().IndexOf(currentPivot) -1)).GlobalPosition, (pc.animationDelta).Clamp01()) + new Vector3(0, playerYOffset, 0);
						pc.RotateMesh(currentPivot.GlobalRotation);
						if (pc.animationDelta >= .95f)
						{
							wantsToMove = false;
							currentPivot = (Node3D)barPivotParent.GetChild(barPivotParent.GetChildren().IndexOf(currentPivot) - 1);
							pc.cb.GlobalPosition = currentPivot.GlobalPosition + new Vector3(0, playerYOffset, 0);

						}
					}
				}
			}
		}
	}

	public override bool ManageInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact"))
		{
			pc.ForceUninteract();
			inUse = false;
		}



			return false;
	}

}
