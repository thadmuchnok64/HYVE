using Godot;
using System;

public partial class NavigableUIElement : Control
{
	public bool isHighlighted = false;
	[Export] AnimationPlayer anim;
	[Export] string animMetaHighlight, animMetaHighlightOut;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void Highlight()
	{
		isHighlighted = true;
		anim.Play(animMetaHighlight);
	}

    public virtual void Unhighlight()
    {
        isHighlighted = false;
        anim.Play(animMetaHighlightOut);
    }
}
