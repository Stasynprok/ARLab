using System;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
	[SerializeField] private Transform _parentTransform;
	[SerializeField] private int _maxCountFishOnScene;
	public float RadiusCircle;
	
	public FishGenerationParameters[] GenerationParameters;
	private List<FishPool> FishPools = new List<FishPool>();
	public bool GenerateFishs = false;
	
	private void OnValidate()
	{
		if(GenerateFishs)
		{
			for(int i = 0; i < 5; i++)
			{
				FishLogic fish = FishPools[0].Get();
				Debug.LogError($"fish:{fish.gameObject.name}");
			}
			
			GenerateFishs = false;
		}
	}
	
	private void Start()
	{
		GeneratePools();
		GenerateFish();
	}
	
	private void GeneratePools()
	{
		for(int i = 0; i < GenerationParameters.Length; i++)
		{
			GameObject fishPrefab = GenerationParameters[i].FishData.FishPrefab;
			int countToCreate = GenerationParameters[i].Count;
			int maxCount = GenerationParameters[i].Count;
			GameObject pool = new GameObject($"Pool{i}");
			pool.transform.parent = _parentTransform;
			
			FishPool fishPool = pool.AddComponent<FishPool>();
			fishPool.Initialize(GenerationParameters[i].FishData, fishPrefab, pool.transform, countToCreate, maxCount);
			FishPools.Add(fishPool);
		}
	}
	
	private void GenerateFish()
	{
		int countFishOnScene = UnityEngine.Random.Range(1, _maxCountFishOnScene + 1);
			
		while(countFishOnScene > 0)
		{
			int randomIndex = UnityEngine.Random.Range(0, FishPools.Count);
			FishLogic fishLogic = FishPools[randomIndex].Get();
			
			if(fishLogic == null)
			{
				continue;
			}
			
			fishLogic.transform.position = GetRandomPointInGlobal();
			fishLogic.ActionOnRelease = OnReleaseFishCheck;
			fishLogic.FuncOnPointDestination = GetRandomPointInGlobal;
			
			float weightFish = UnityEngine.Random.Range(100f, fishLogic.MaxWeight);
			fishLogic.SetFishParameters(weightFish);
			fishLogic.StartFish();
			
			countFishOnScene--;
		}
	}
	
	private void OnReleaseFishCheck()
	{
		int countFishOnScene = 0;
		
		for(int i = 0; i < FishPools.Count; i++)
		{
			countFishOnScene += FishPools[i].CountActive;
		}
		
		int newCountFishOnScene = UnityEngine.Random.Range(1, _maxCountFishOnScene + 1);
		
		if(countFishOnScene > newCountFishOnScene)
		{
			return;
		}
		
		int countFishToSpawn = newCountFishOnScene - countFishOnScene;
		
		while(countFishToSpawn > 0)
		{
			int randomIndex = UnityEngine.Random.Range(0, FishPools.Count);
			FishLogic fishLogic = FishPools[randomIndex].Get();
			
			if(fishLogic == null)
			{
				continue;
			}
			
			fishLogic.transform.position = GetRandomPointInGlobal();
			fishLogic.ActionOnRelease = OnReleaseFishCheck;
			fishLogic.FuncOnPointDestination = GetRandomPointInGlobal;
			
			float weightFish = UnityEngine.Random.Range(100f, fishLogic.MaxWeight);
			fishLogic.SetFishParameters(weightFish);
			fishLogic.StartFish();
			countFishToSpawn--;
		}
	}
	
	
	private Vector3 GetRandomPointInGlobal()
	{
		// Получаем случайный угол
		float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

		// Вычисляем координаты точки на окружности
		float x = RadiusCircle * 0.5f * Mathf.Cos(angle);
		float y = RadiusCircle * 0.5f * Mathf.Sin(angle);
		
		Vector3 point = new Vector3(x, 0, y);
		point = transform.TransformPoint(point);
		
		return point;
	}
	
	[Serializable]
	public class FishGenerationParameters
	{
		public int Count;
		public FishData FishData;
	}
}
