using Godot;
using System;

public partial class Centaur_Recoil : IHE_Recoil
{
	[Export] string decapitationAnimMeta = "decapitate";
	[Export] float decapitationRecoilTime = 1.25f;

	bool decapitatedThisInstance = false;
	public override EnemyState Enter(Enemy enemy)
	{
		decapitatedThisInstance = false;
		base.Enter(enemy);
		if (enem is Centaur)
		{
			if (((Centaur)enem).CheckForPrimaryDecapitation())
			{
				enem.anim.Set($"parameters/{animMetaState}/{decapitationAnimMeta}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
				enem.anim.Set("parameters/moving/Limp/blend_amount", 1.0f);
				enem.anim.Set("parameters/idle/Limp/blend_amount", 1.0f);
				decapitatedThisInstance = true;
			}

		}
		else
		{
			GD.PrintErr("what the fuck");
		}

			return null;
	}

	public override EnemyState Process(double delta)
	{
		timer += (float)delta;
		if (decapitatedThisInstance)
		{
			if (timer > decapitationRecoilTime)
			{
				return movingState;
			}
		}
		else
		{
			if (timer > recoilTime)
			{
				return movingState;
			}
		}
		return null;
	}

}
