using Godot;
using System;

public partial class PC_Inventory : PCState
{
    [Export] PCState idleState;

    public override PCState ManageInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Inventory"))
        {
            bool open = stateMachine.TriggerInventory();
            if (!open)
                return idleState;
        }
        return base.ManageInput(@event);
    }

    public override PCState Enter()
    {
        anim.Set("parameters/interact/Transition/transition_request", animMeta);
        return base.Enter();
    }
}
