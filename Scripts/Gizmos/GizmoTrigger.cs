using Godot;
using System;

public partial class GizmoTrigger : Area3D
{
    [Export] public InteractableObject interactable;
    //[Signal] public delegate void TriggerGizmoEventHandler(PCStateMachine pc);

    public void TryTriggerGizmo(PCStateMachine pc)
    {
        //EmitSignal(SignalName.TriggerGizmo, pc);
        interactable.TriggerGizmo(pc);
    }
}
