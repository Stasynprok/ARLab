using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishListController : MonoBehaviour
{
	[SerializeField] private GameObject _UIObjectFishList;
	[SerializeField] private GameObject _prefabBlock;
	[SerializeField] private GameObject _contentBlocks;
	[SerializeField] private GameObject _showListButton;
	private List<FishParameters> _fishsListParameters = new List<FishParameters>();
	private List<FishBlockInfoController> _fishBlockInfos = new List<FishBlockInfoController>();
	
	public void AddFishInList(FishParameters fishParameters)
	{
		if(_fishsListParameters.Count < 10)
		{
			_fishsListParameters.Add(fishParameters);
			UpdateBlocksInList();
			return;
		}
		
		_fishsListParameters.RemoveAt(0);
		_fishsListParameters.Add(fishParameters);
		
		UpdateBlocksInList();
	}
	
	private void UpdateBlocksInList()
	{
		int countFishInList = _fishsListParameters.Count;
		
		if(_fishBlockInfos.Count != countFishInList)
		{
			int countNewBlocks = countFishInList - _fishBlockInfos.Count;
			
			for(int i = 0; i < countNewBlocks; i++)
			{
				AddBlockInList();
			}
		}
		countFishInList--;
		for(int i = 0; i < _fishBlockInfos.Count; i++)
		{
			FishBlockInfoController fishBlockInfo = _fishBlockInfos[i];
			FishParameters fishParameters = _fishsListParameters[countFishInList];
			
			fishBlockInfo.SetFishInformationInBlock(fishParameters.FishImage, fishParameters.Name, fishParameters.Weight.ToString());
			
			countFishInList--;
		}
	}
	
	private void AddBlockInList()
	{
		GameObject blockInfo = Instantiate(_prefabBlock, _contentBlocks.transform);
		FishBlockInfoController fishBlockInfo = blockInfo.GetComponent<FishBlockInfoController>();
		_fishBlockInfos.Add(fishBlockInfo);
	}
	
	public void ShowList()
	{
		_UIObjectFishList.SetActive(true);
		_showListButton.SetActive(false);
	}
	
	public void CloseList()
	{
		_UIObjectFishList.SetActive(false);
		_showListButton.SetActive(true);
	}
}
