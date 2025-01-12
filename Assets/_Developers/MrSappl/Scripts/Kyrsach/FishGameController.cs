using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGameController : MonoBehaviour
{
	public static FishGameController Instance { get; private set; }
	[SerializeField] private FishingRodController _fishingRodController;
	[SerializeField] private FishInfoPanel _fishInfoUI;
	[SerializeField] private FishListController _fishListController;
	private FishParameters _currentFish;
	public GameObject LakeObject;
	public bool FishIsBite = false;
	public bool BobberInLake = false;
	
	public bool ActivateBobberAnimation = false;
	
	
	private void Awake() 
	{ 		
		if (Instance != null && Instance != this) 
		{ 
			Destroy(this); 
		} 
		else 
		{ 
			Instance = this; 
		} 
	}
	
	public Vector3 GetBobberPositionOnLake()
	{
		Vector3 bobberGlobalPosition = _fishingRodController.BobberGlobalPosition;
		
		Vector3 bobberPosition = LakeObject.transform.InverseTransformPoint(bobberGlobalPosition);
		
		return bobberPosition;
	}
	
	public bool CanFishBite()
	{
		return !FishIsBite && BobberInLake;
	}
	
	public bool FinsOnBobber()
	{
		return _currentFish != null;
	}
	
	public void OnBiteFish(FishParameters fishLogic)
	{
		Debug.LogError("OnBiteFish");
		_currentFish = fishLogic;
		
		_fishingRodController.StartBobberBiteAnimation();
	}
	
	public void OnCoughtFish()
	{
		if(_currentFish == null)
		{
			return;
		}
		_fishInfoUI.SetParameters(_currentFish);
		_fishInfoUI.TranslateFishGameObjectToScreen();
		_fishInfoUI.Activate();
		
		_fishListController.AddFishInList(_currentFish);
		_currentFish = null;
	}
	
	
}
public class FishParameters
{
	public Sprite FishImage;
	public string Name;
	public float Weight;
	public Transform FishTransform;
	public Action FishReleaseToPool;
	
	public FishParameters(Sprite fishImage, string name, float weight, Transform fishTransform, Action fishReleaseToPool)
	{
		FishImage = fishImage;
		Name = name;
		Weight = weight;
		FishTransform = fishTransform;
		FishReleaseToPool = fishReleaseToPool;
	}
}
