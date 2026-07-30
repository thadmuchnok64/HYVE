using Godot;
using System;
using System.Collections.Generic;

public partial class HUDManager : Control
{
	[Export] public AnimationTree anim;
	[Export] AudioStreamPlayer2D aud;

	[Export] RichTextLabel dialogueBox;
	[Export] UIState currentState;

	bool inventoryOpen = false;


	[Export] TextureProgressBar staminaBar;
	[Export] TextureProgressBar healthBar;
	[Export] TextureProgressBar posBar;
	[Export] AudioStream inventoryInSFX, inventoryOutSFX;
	[Export] EquipmentScreen equipmentScreen;
	[Export] LoreMenu loreMenu;

	[Export] float trackerOffset = 32;

	[Export] Control tracker;

	Queue<string> pendingDialogues;
	bool dialoguePending = false;



	public static HUDManager instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (instance != null)
		{
			GD.Print("wtf");
		}
		else { instance = this; }
		pendingDialogues = new Queue<string>();
		AnimateOutDialogue();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (dialoguePending)
		{
			AnimateInDialogue();
		}
	}

	public void QueueDialogue(string msg)
	{
		pendingDialogues.Enqueue(msg);
		dialoguePending = true;
	}

	private void AnimateInDialogue()
	{
		dialogueBox.Text = pendingDialogues.Dequeue();
		anim.Set("parameters/Main/Dialogue/transition_request", "in");
    }
    private void AnimateOutDialogue()
    {
        anim.Set("parameters/Main/Dialogue/transition_request", "out");
    }

    public bool TryAdvanceDialogue() // returns false if the dialogue has reached the end of the queue
	{
		if (pendingDialogues.Count <= 0)
		{
			dialoguePending = false;
			AnimateOutDialogue();
			return false;
		}
        dialogueBox.Text = pendingDialogues.Dequeue();
        if (pendingDialogues.Count <= 0)
        {
            dialoguePending = false;
            AnimateOutDialogue();
            return false;
        }
		return true;
    }
	
	public void SnapTrackerToPoint(Vector3 globalPos,Camera3D cam)
	{
		tracker.Position = cam.UnprojectPosition(globalPos)-(tracker.Size/2f) - new Vector2(0,trackerOffset);
	}

	public void ShowTracker(bool showing)
	{
		if (showing)
			anim.Set("parameters/Main/LockOn/transition_request", "in");
		else
			anim.Set("parameters/Main/LockOn/transition_request", "out");
	}

	public bool ToggleInventory(InventoryManager inventoryToOpen)
	{
		inventoryOpen = !inventoryOpen;
		if (inventoryOpen)
		{
			aud.Stream = inventoryInSFX;
			aud.Play();
			equipmentScreen.RefreshInventory(inventoryToOpen);
			SwitchState(equipmentScreen);
		}
		else
		{
			aud.Stream = inventoryOutSFX;
			aud.Play();
            SwitchState(null);
        }
        return inventoryOpen;
	}

	public void ToggleLore(LoreNote note)
	{
		if(currentState == null)
		{
			SwitchState(loreMenu);
			loreMenu.Populate(note);
		}
		else
		{
			SwitchState(null);
		}
	}


            #region Bars
            public void SetStamina(float current, float max)
	{
		staminaBar.MaxValue = max;
		staminaBar.Value = current;
	}

	public void SetHealth(float current, float max)
	{
		healthBar.MaxValue = max;
		healthBar.Value = current;
	}

	public void SetPosture(float current, float max)
	{
		posBar.MaxValue = max;
		posBar.Value = current;
	}
    #endregion


    #region State Machine Functions
    public void SwitchState(UIState deltaState)
	{
		if (currentState != null)
			currentState.Exit();
		if(deltaState != null)
		{
			currentState = deltaState.Enter();
		} else { currentState = null; }
	}

    public override void _Input(InputEvent @event)
    {
		if (currentState == null)
			return;
        UIState state = currentState.ManageInput(@event);
        if (state != null)
            SwitchState(state);
    }
    #endregion
}
