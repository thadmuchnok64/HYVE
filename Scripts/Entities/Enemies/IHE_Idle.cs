using Godot;
using System;

public partial class IHE_Idle: EnemyState
{
	[Export] float detectionDistance;
	[Export] EnemyState seekingState;
	[Export] EnemyState recoilState;
    [Export] EnemyState postureBreakState;




    public override EnemyState Enter(Enemy enemy)
	{
		return base.Enter(enemy);
	}

	public override EnemyState DetectPlayerEvent()
	{
		return seekingState;

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
}
