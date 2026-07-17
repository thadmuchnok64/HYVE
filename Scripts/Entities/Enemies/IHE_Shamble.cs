using Godot;
using System;

public partial class IHE_Shamble : EnemyState
{
	[Export] float speed;
	[Export] EnemyState idleState;
	[Export] EnemyState recoilState;
	[Export] EnemyState postureBreakState;
	[Export] IHE_Attack attackState;
	[Export] float distanceToAttack = 2f;
	[Export] float maxTurnPerSec = 90f;
    public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		enem.nav.TargetPosition = GameMaster.Instance.GetPlayer().Position;

		return null;
	}
	public override EnemyState PhysicsProcess(double delta)
	{
		//enem.cb.Velocity = (enem.nav.GetNextPathPosition() - enem.cb.GlobalPosition).Normalized() * speed;
		//enem.cb.MoveAndSlide();
		//enem.cb.Rotate();+
		float desiredAngle = enem.meshRoot.Basis.Z.SignedAngleTo((enem.nav.GetNextPathPosition() - enem.meshRoot.GlobalPosition).Normalized(), Vector3.Up);
		//float frameAngle = desiredAngle
		float clampedAngle = Mathf.Clamp(desiredAngle, -(float)delta * maxTurnPerSec*Mathf.Pi, (float)delta * maxTurnPerSec * Mathf.Pi);
		enem.meshRoot.RotateY(clampedAngle);
		enem.cb.Velocity = enem.meshRoot.Basis.Z* speed;
        enem.cb.MoveAndSlide();
        //if (enem.cb.Velocity.Length() > .2f)
        //{
        //enem.meshRoot.LookAt(enem.cb.Position - enem.cb.Velocity.Normalized() * 5, Vector3.Up);
        //enem.meshRoot.Rotation = new Vector3(0, enem.meshRoot.Rotation.Y, 0);
        //}


        if (enem.nav.TargetPosition.DistanceTo(enem.cb.GlobalPosition) < .7f)
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
		if (enem.nav.TargetPosition.DistanceTo(GameMaster.Instance.GetPlayer().GlobalPosition) > distanceToAttack)
		{
            enem.nav.TargetPosition = GameMaster.Instance.GetPlayer().Position;

        }
        return base.Process(delta);
    }
}
