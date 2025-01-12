using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishBlockInfoController : MonoBehaviour
{
	[SerializeField] private Image _fishImage;
	[SerializeField] private TMP_Text _fishName;
	[SerializeField] private TMP_Text _fishWeight;
	
	public void SetFishInformationInBlock(Sprite fishImage, string fishName, string fishWeight)
	{
		if(_fishImage)
		{
			_fishImage.sprite = fishImage;
		}
		
		if(_fishName)
		{
			_fishName.text = fishName;
		}
		
		if(_fishWeight)
		{
			_fishWeight.text = fishWeight;
		}
	}
}