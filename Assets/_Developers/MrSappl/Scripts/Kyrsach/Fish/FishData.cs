using UnityEngine;

[CreateAssetMenu(fileName = "New FishData", menuName = "Fish Data", order = 51)]
public class FishData : ScriptableObject
{
	public GameObject FishPrefab;
	public string Name;
	public Sprite Icon;
}
