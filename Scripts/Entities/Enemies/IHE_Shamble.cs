using Godot;
using System;

public partial class IHE_Shamble : EnemyState
{
	[Export] float speed;
	[Export] EnemyState idleState;
	[Export] EnemyState recoilState;
	[Export] EnemyState postureBreakState;


    public override EnemyState Enter(Enemy enemy)
	{
		base.Enter(enemy);
		enem.nav.TargetPosition = enem.player.Position;

		return null;
	}
	public override EnemyState PhysicsProcess(double delta)
	{
		enem.cb.Velocity = (enem.nav.GetNextPathPosition() - enem.cb.GlobalPosition).Normalized() * speed;
		enem.cb.MoveAndSlide();

		if (enem.cb.Velocity.Length() > .2f)
		{
			enem.meshRoot.LookAt(enem.cb.Position - enem.cb.Velocity.Normalized() * 5, Vector3.Up);
			enem.meshRoot.Rotation = new Vector3(0, enem.meshRoot.Rotation.Y, 0);
		}


		if (enem.nav.TargetPosition.DistanceTo(enem.cb.GlobalPosition) < .7f)
		{
			return idleState;
		}

		enem.nav.TargetPosition.DistanceTo(enem.cb.GlobalPosition);
		
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
}
