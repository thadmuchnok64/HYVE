using Godot;
using System;

public partial class Entity : Node3D
{

	[Export] protected int maxHealth;
	[Export] protected int maxPosture;
	public float health;
	public float posture;
	public bool alive = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		health = maxHealth;
		posture = maxPosture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;
		if(alive && health < 0)
		{
			Die();
		}
	}

    public virtual void TakePostureDamage(float damage)
    {
        posture -= damage;
		GD.Print("posture " + posture);
        if (posture < 0)
        {
            //break
        }
    }

	public virtual void Die()
	{
		alive = false;
	}
}
