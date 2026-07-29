using Godot;
using System;

public partial class UIState : Control
{
    //VARIABLES
    protected int navIndex1D = 0;
    protected Vector2 navIndex2D = Vector2.Zero;

    public virtual UIState Enter()
    {
        navIndex1D = 0;
        navIndex2D = Vector2.Zero;
        return this;
        //do animation here
    }

    public virtual UIState Exit()
    {
        return null;
    }

    public virtual UIState ManageInput(InputEvent @event)
    {
        if (Input.IsActionPressed("UIUp"))
            return NavUp();
        if (Input.IsActionPressed("UIDown"))
            return NavDown();
        if (Input.IsActionPressed("UILeft"))
            return NavLeft();
        if (Input.IsActionPressed("UIRight"))
            return NavRight();
        if (Input.IsActionPressed("UIBack"))
            return NavBack();
        if (Input.IsActionPressed("UISelect"))
            return NavForward();
        return null;
    }

    // Common navigation events

    public virtual UIState NavUp() { return null; }
    public virtual UIState NavDown() { return null; }
    public virtual UIState NavLeft() { return null; }
    public virtual UIState NavRight() { return null; }
    public virtual UIState NavBack() { return null; }
    public virtual UIState NavForward() { return null; } // also used for selecting stuff

    // Common helpers


}
