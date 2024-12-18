using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishInfoPanel : MonoBehaviour
{
	[SerializeField] private Transform _fishTransformOnScreen;
	[SerializeField] private GameObject _ui;
	[SerializeField] private TMP_Text _fishName;
	[SerializeField] private TMP_Text _fishWeight;
	private FishParameters _fishParameters;
	
	public void SetParameters(FishParameters fishObject)
	{
		_fishParameters = fishObject;
		_fishName.text = _fishParameters.Name;
		_fishWeight.text = _fishParameters.Weight.ToString();
	}
	
	public void Activate()
	{
		_ui.SetActive(true);
	}
	
	public void Deactivate()
	{
		_ui.SetActive(false);
	}
	
	public void TranslateFishGameObjectToScreen()
	{
		
		_fishParameters.FishTransform.parent = _fishTransformOnScreen;
		_fishParameters.FishTransform.localPosition = Vector3.zero;
		_fishParameters.FishTransform.localRotation = Quaternion.identity;
	}
	
	public void OnCloseButton()
	{
		Debug.Log("OnCloseButton");
		_fishParameters.FishReleaseToPool.Invoke();
		Debug.Log("FishReleaseToPool");
		Deactivate();
		Debug.Log("Deactivate");
	}
}
