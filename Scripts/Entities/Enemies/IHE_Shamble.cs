using Godot;
using System;

public partial class IHE_Shamble : EnemyState
{
	[Export] float speed;
	[Export] EnemyState idleState;
	[Export] EnemyState recoilState;
	[Export] EnemyState postureBreakState;
	[Export] IHE_Attack attackState;
	[Export] float optimalDistance = 2f;
	[Export] float maxTurnPerSec = 90f;
    public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		enem.nav.TargetPosition = GameMaster.Instance.GetPlayer().Position;

		return null;
	}
	public override EnemyState PhysicsProcess(double delta)
	{

		float desiredAngle = enem.meshRoot.Basis.Z.SignedAngleTo((enem.nav.GetNextPathPosition() - enem.meshRoot.GlobalPosition).Normalized(), Vector3.Up);
		float clampedAngle = Mathf.Clamp(desiredAngle, -(float)delta * maxTurnPerSec*Mathf.Pi, (float)delta * maxTurnPerSec * Mathf.Pi);
		enem.meshRoot.RotateY(clampedAngle);
		enem.cb.Velocity = enem.meshRoot.Basis.Z* speed;
        enem.cb.MoveAndSlide();


        if (enem.GlobalPosition.DistanceTo(GameMaster.Instance.GetPlayer().GlobalPosition) < optimalDistance)
		{
			return idleState;
		}

		//enem.nav.TargetPosition.DistanceTo(enem.cb.GlobalPosition);
		
		enem.anim.Set(animMeta, Mathf.Clamp(enem.cb.Velocity.Length() / speed, 0, 1));
		return null;
	}

    public override EnemyState HitEvent()
    {
        if (enem.posture <= 0)
        {
            return postureBreakState;
        }
        else
        {
            return recoilState;
        }
    }

    public override EnemyState Process(double delta)
    {
		if (attackState.IsAttackValid())
			return attackState;
		if (enem.nav.TargetPosition.DistanceTo(GameMaster.Instance.GetPlayer().GlobalPosition) > optimalDistance)
		{
            enem.nav.TargetPosition = GameMaster.Instance.GetPlayer().Position;

        }
        return base.Process(delta);
    }
}
