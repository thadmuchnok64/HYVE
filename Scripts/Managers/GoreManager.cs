using Godot;
using System;
using System.Collections.Generic;


public enum GoreType { FLESH, BONE, BRAIN}
public enum BloodDecalType { SMALL, LARGE, MASSIVE }

public partial class GoreManager : Node3D
{
	[Export] float launchVelocity = 4f;
	[Export] Godot.Collections.Array<PackedScene> fleshPrefabs;
	[Export] Godot.Collections.Array<PackedScene> bonePrefabs;
	[Export] Godot.Collections.Array<PackedScene> brainPrefabs;

	[Export] int maxSmallBloodDecals = 48;
	[Export] int maxLargeBloodDecals = 32;

	[Export] PackedScene smallBloodDecal;
	[Export] PackedScene largeBloodDecal;

	[Export] AudioStream splatSFX;

    List <DecalEffect> smallBloodDecalList,largeBloodDecalList;
	[Export] float smallBloodSplatVolume = .5f;
    [Export] float largeBloodSplatVolume = .9f;

    int smallBloodItr, largeBloodItr;

	public static GoreManager Instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(Instance != null)
		{
			GD.Print("Multiple goremanagers!!! wtf is wrong with you");
			return;
		}
		Instance = this;

		// Initialize object pools

		smallBloodDecalList = new List<DecalEffect>();
		largeBloodDecalList = new List<DecalEffect>();

		for(int i = 0; i < maxSmallBloodDecals; i++)
		{
			var dec = (DecalEffect)smallBloodDecal.Instantiate();
			AddChild(dec);
			smallBloodDecalList.Add(dec);
			dec.Visible = false;
		}
        for (int i = 0; i < maxLargeBloodDecals; i++)
        {
            var dec = (DecalEffect)largeBloodDecal.Instantiate();
            AddChild(dec);
            largeBloodDecalList.Add(dec);
            dec.Visible = false;

        }
    }


    #region GORE
    public void RequestGoreAtLocation(GoreType goreType, Vector3 globalPosition)
	{
		switch (goreType)
		{
			case GoreType.FLESH:
				LaunchGore(fleshPrefabs.PickRandom(), globalPosition);
				break;
			case GoreType.BONE:
				LaunchGore(bonePrefabs.PickRandom(), globalPosition);
				break;
			case GoreType.BRAIN:
				LaunchGore(brainPrefabs.PickRandom(), globalPosition);
				break;
		}
	}

	public void LaunchGore(PackedScene prefab, Vector3 globalPosition)
	{
		var gore = prefab.Instantiate();
		AddChild(gore);
		((Node3D)gore).GlobalPosition = globalPosition;
		((RigidBody3D)gore).LinearVelocity = StaticHelpers.RandomVector() * launchVelocity;
	}

    #endregion

    #region BLOOD

	public void RequestBloodSplatAtLocation(Vector3 globalPos,BloodDecalType type)
	{
		PackedScene dec;
		switch (type)
		{
			case BloodDecalType.SMALL:
                SmallBlood(globalPos);
                break;
			case BloodDecalType.LARGE:
				LargeBlood(globalPos);
                break;
			default:
				SmallBlood(globalPos);
                break;
		}
    }

	public void SmallBlood(Vector3 pos)
	{
		smallBloodDecalList[smallBloodItr].Reset();
		smallBloodDecalList[smallBloodItr].GlobalPosition = pos;
		smallBloodDecalList[smallBloodItr].Visible = true;
        smallBloodItr++;
		SoundManager.Instance.RequesetSFXSoundAtLocation(splatSFX, pos, smallBloodSplatVolume);
		if(smallBloodItr >= smallBloodDecalList.Count)
		{
			smallBloodItr = 0;
        }
	}

    public void LargeBlood(Vector3 pos)
    {
        largeBloodDecalList[smallBloodItr].Reset();
        largeBloodDecalList[smallBloodItr].GlobalPosition = pos;
        largeBloodDecalList[smallBloodItr].Visible = true;
        largeBloodItr++;
        SoundManager.Instance.RequesetSFXSoundAtLocation(splatSFX, pos, largeBloodSplatVolume);

        if (largeBloodItr >= largeBloodDecalList.Count)
        {
            largeBloodItr = 0;
        }
    }

    #endregion
}
