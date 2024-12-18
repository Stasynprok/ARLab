using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

public class FishPool : MonoBehaviour
{
	private Transform _parent;
	private GameObject _objectPrefab;
	private FishData _fishData;
	
	public int CountActive =>_objectPool.CountActive;
	public int CountInActive =>_objectPool.CountInactive;
	public int CountAll =>_objectPool.CountAll;
	private ObjectPool<FishLogic> _objectPool;
	private int _maxSize;
		
	public void Initialize(FishData fishData, GameObject prefabObjects, Transform parentForObjects, int counForCreate, int maxSize)
	{
		_fishData = fishData;
		_parent = parentForObjects;
		_objectPrefab = prefabObjects;
		_maxSize = maxSize;
		_objectPool = new ObjectPool<FishLogic>(Create, OnGet, OnRelease, OnPoolDestroy, false, counForCreate, maxSize);
	}
	
	private FishLogic Create()
	{
		GameObject fish = Instantiate(_objectPrefab, _parent);
		
		FishLogic fishLogic = fish.GetComponent<FishLogic>();
		fishLogic.InstantiateFromPool(Release, _fishData);
		
		return fishLogic;
	}
	
	private void OnGet(FishLogic instance)
	{
		instance.transform.SetParent(null);
		instance.gameObject.SetActive(true);
	}
	
	private void OnRelease(FishLogic instance)
	{
		instance.transform.SetParent(_parent);
		instance.gameObject.SetActive(false);
	}
	
	private void OnPoolDestroy(FishLogic instance)
	{
		Destroy(instance.gameObject);
	}
	
	public FishLogic Get()
	{
		Debug.LogError($"_objectPool.CountAll: {_objectPool.CountAll}");
		Debug.LogError($"_maxSize: {_maxSize}");
		if(_objectPool.CountAll >= _maxSize)
		{
			return null;
		}
		
		return _objectPool.Get();
	}
	
	public void Release(FishLogic instance)
	{
		instance.gameObject.SetActive(false);
		instance.gameObject.transform.SetParent(_parent);
		instance.gameObject.transform.localPosition = Vector3.zero;
		Debug.LogError("instance.gameObject.transform.");
		_objectPool.Release(instance);
		Debug.LogError("Release");
	}
	
	public void Clear()
	{
		_objectPool.Clear();
	}
}
