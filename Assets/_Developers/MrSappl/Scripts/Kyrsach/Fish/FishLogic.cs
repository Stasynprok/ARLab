using System;
using DG.Tweening;
using UnityEngine;

public class FishLogic : MonoBehaviour
{
	public Action<FishLogic> releaseToPool;
	public Action ActionOnRelease;
	public Func<Vector3> FuncOnPointDestination;
	public GameObject Visual;
	
	public float MaxWeight = 3000f;
	public float Speed = 1f;
	public float SpeedLookAt = 1f;
	
	private FishData _fishData;
	private float _weight;
	private Sequence _sequence;
	private Vector3 _destinationPoint;
	
	private float _timer;
	
	private bool _isBite = false;
	private bool _fishInGame = false;
	
	public FishData FishInformation { get { return _fishData; } }
	
	private void Update()
	{
		if(_isBite || !FishGameController.Instance.CanFishBite() || !_fishInGame)
		{
			_timer = 0f;
			return;
		}
		
		if(_timer <= 0.0f)
		{
			_timer = UnityEngine.Random.Range(5f, 10f);
		}
		
		if(_timer > 0.0f)
		{
			_timer -= Time.deltaTime;
		}
		
		if(_timer < 0.0f)
		{
			_isBite = UnityEngine.Random.Range(0, 100) < 5;
		
			if(_isBite)
			{
				OnBite();
			}
		}
	}
	
	private void OnBite()
	{
		if(FishGameController.Instance.FishIsBite)
		{
			return;
		}
		FishGameController.Instance.FishIsBite = true;
		KillAnimation();
		Vector3 bobberPositionOnLake = FishGameController.Instance.GetBobberPositionOnLake();
		bobberPositionOnLake = new Vector3(bobberPositionOnLake.x, 0f, bobberPositionOnLake.z);
		
		_destinationPoint = FishGameController.Instance.LakeObject.transform.TransformPoint(bobberPositionOnLake);
		MoveToBobber();
	}

	public void InstantiateFromPool(Action<FishLogic> action, FishData fishData)
	{
		releaseToPool = action;
		_fishData = fishData;
	}
	
	public void SetFishParameters(float weight)
	{
		_weight = weight;
		float onePercent = MaxWeight * 0.01f;
		float percent = (_weight / onePercent) * 0.01f;
		Speed = Mathf.Lerp(3f, 5.0f, percent);
		
		float size = Mathf.Lerp(0.01f, 0.1f, percent);
		
		Visual.transform.localScale = Vector3.one * size;
	}
	
	public void SetFunc(Func<Vector3> func)
	{
		FuncOnPointDestination = func;
		_destinationPoint = FuncOnPointDestination();
	}
	
	public void StartFish()
	{
		_timer = UnityEngine.Random.Range(5f, 10f);
		_fishInGame = true;
		NewPoint();
		MoveToPoint();
	}
	
	private void MoveToPoint()
	{
		if(_isBite)
		{
			return;
		}
		_sequence = DOTween.Sequence();
		_sequence.Append(transform.DOMove(_destinationPoint, Speed));
		_sequence.Join(transform.DOLookAt(_destinationPoint, SpeedLookAt));
		_sequence.OnComplete(() => {
				Debug.LogError("OnComplete");
				NewPoint();});
	}
	private void MoveToBobber()
	{
		_sequence = DOTween.Sequence();
		_sequence.Append(transform.DOMove(_destinationPoint, Speed));
		_sequence.Join(transform.DOLookAt(_destinationPoint, SpeedLookAt));
		_sequence.OnUpdate(CheckBobber);
		
		_sequence.OnComplete(OnFishDestinationBobber);
	}
	
	private void CheckBobber()
	{
		if(FishGameController.Instance.FinsOnBobber() || !FishGameController.Instance.BobberInLake)
		{
			KillAnimation();
			_isBite = false;
			_timer = 0f;
			NewPoint();
		}
	}
	
	private void OnFishDestinationBobber()
	{
		KillAnimation();
		if(FishGameController.Instance.FinsOnBobber())
		{
			_isBite = false;
			NewPoint();
			return;
		}
		Debug.LogError("OnFishDestinationBobber");
		FishParameters fishParameters = new FishParameters(_fishData.Name, _weight, gameObject.transform, ReturToPool);
		FishGameController.Instance.OnBiteFish(fishParameters);
	}
	
	private void KillAnimation()
	{
		Debug.LogWarning("KillAnimation");
		// if(_sequence == null)
		// {
		// 	return;
		// }
		_sequence.Kill(false);
		_sequence = null;
	}
	
	private void NewPoint()
	{
		_destinationPoint = FuncOnPointDestination();
		MoveToPoint();
	}
	public void ReturToPool()
	{
		_isBite = false;
		_fishInGame = false;
		releaseToPool.Invoke(this);
		ActionOnRelease.Invoke();
	}
}
