using Godot;
using System;

public partial class Door : InteractableObject
{
    [Export] Node3D playerWarpPosition;
    [Export] CollisionShape3D colliderToDisable;
    [Export] AnimationPlayer anim;
    [Export] float timeToFreePlayer = 2f;
    [Export] AudioStream prySfx,pryOpenSfx;
    [Export] string itemNeededToProgress = "Crowbar";

    PCStateMachine pc;

    float activeTimer = 0;
    bool active = false;
    bool used = false;
    public override void TriggerGizmo(PCStateMachine pc)
    {
        if (used)
        {
            pc.ForceUninteract();
            return;
        }
        if (pc.inventory.DoesHaveItem(itemNeededToProgress)) // check inventory for object here
        {
            used = true;
            interactSuccess = true;
            colliderToDisable.Disabled = true;
            activeTimer = 0;
            active = true;
            anim.Active = true;
            anim.Play("PryOpen");
            this.pc = pc;
            pc.cb.GlobalPosition = playerWarpPosition.GlobalPosition;
            pc.RotateMesh(playerWarpPosition.GlobalRotation);
        }
        else
        {
            interactSuccess = false;
            HUDManager.instance.QueueDialogue(defaultMessage);
        }
    }

    public override bool ManageInput(InputEvent @event)
    {
        if (@event.IsActionPressed("Interact"))
        {
            return !HUDManager.instance.TryAdvanceDialogue();
        }
        return false;
    }

    public override void _Process(double delta)
    {
        if (active)
        {
            activeTimer += (float)delta;
            if (activeTimer > timeToFreePlayer)
            {
                pc.ForceUninteract();
                active = false;
            }
        }
    }

    public void PlayPrySFX()
    {
        SoundManager.Instance.RequesetSFXSoundAtLocation(prySfx, GlobalPosition);
    }


    public void PlayPryOpenSFX()
    {
        SoundManager.Instance.RequesetSFXSoundAtLocation(pryOpenSfx, GlobalPosition);
    }


}
