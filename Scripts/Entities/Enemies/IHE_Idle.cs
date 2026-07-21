using Godot;
using System;

public partial class IHE_Idle: EnemyState
{
	[Export] float detectionDistance;
	[Export] EnemyState seekingState;
	[Export] EnemyState recoilState;
    [Export] EnemyState postureBreakState;
	[Export] IHE_Attack attackState;
	[Export] float distanceToFollow = 3f;
	[Export] float maxTurnPerSec = 1f;





	public override EnemyState Enter(Enemy enemy)
	{
		return base.Enter(enemy);
	}

	public override EnemyState DetectPlayerEvent()
	{
		if (enem.GlobalPosition.DistanceTo(GameMaster.Instance.GetPlayer().GlobalPosition) >= distanceToFollow)
		{
			return seekingState;
		}
		return null;

	}

	public override EnemyState HitEvent()
	{
		if(enem.posture <= 0)
		{
			return postureBreakState;
		}
		else
        {
            return recoilState;
        }
    }

	public override EnemyState PhysicsProcess(double delta)
	{
		float desiredAngle = enem.meshRoot.Basis.Z.SignedAngleTo((GameMaster.Instance.GetPlayer().GlobalPosition - enem.meshRoot.GlobalPosition).Normalized(), Vector3.Up);
		float clampedAngle = Mathf.Clamp(desiredAngle, -(float)delta * maxTurnPerSec * Mathf.Pi, (float)delta * maxTurnPerSec * Mathf.Pi);
		enem.meshRoot.RotateY(clampedAngle);
		return base.PhysicsProcess(delta);

	}

	public override EnemyState Process(double delta)
	{
		if (attackState != null)
		{
			if (attackState.IsAttackValid())
				return attackState;
		}
		if (enem.GlobalPosition.DistanceTo(GameMaster.Instance.GetPlayer().GlobalPosition) >= distanceToFollow)
		{
			return seekingState;
		}
		return base.Process(delta);
	}
}
