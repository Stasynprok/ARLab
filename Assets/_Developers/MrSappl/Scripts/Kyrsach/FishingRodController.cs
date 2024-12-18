using UnityEngine;
using DG.Tweening;

public class FishingRodController : MonoBehaviour
{
	[SerializeField] private GameObject _fishingRod;
	[SerializeField] private GameObject _bobber;
	[SerializeField] private Transform _bobberInPlayerPosition;
	[SerializeField] private Transform _bobberInPlayerPositionByBite;
	private Sequence _sequence;
	public Vector3 BobberGlobalPosition { get { return _bobber.transform.position; } }
	
	public void Activate()
	{
		_fishingRod.SetActive(true);
		_bobber.SetActive(true);
	}
	
	public void Deactivate()
	{
		_fishingRod.SetActive(false);
		_bobber.SetActive(false);
	}
	
	public void LaunchBobber(Vector3 globalPosition)
	{
		_bobber.transform.parent = null;
		_bobber.transform.position = globalPosition;
		_bobber.transform.rotation = Quaternion.Euler(Vector3.zero);
		FishGameController.Instance.BobberInLake = true;
	}
	
	public void StartBobberBiteAnimation()
	{
		BobberAnimationDown();
	}
	
	private void BobberAnimationDown()
	{
		_sequence = DOTween.Sequence();
		_sequence.Append(_bobber.transform.DOMove(BobberGlobalPosition - Vector3.up * 0.1f, 0.5f));
		_sequence.OnComplete(BobberAnimationUp);
	}
	private void BobberAnimationUp()
	{
		_sequence = DOTween.Sequence();
		_sequence.Append(_bobber.transform.DOMove(BobberGlobalPosition + Vector3.up * 0.1f, 1f));
		_sequence.OnComplete(BobberAnimationDown);
	}
	
	private void KillAnimation()
	{
		if(_sequence == null)
		{
			return;
		}
		_sequence.Kill();
		_sequence = null;
	}
	
	public void RaiseBobber()
	{	
		KillAnimation();
		CoughtFish();
		_bobber.transform.parent = _bobberInPlayerPosition;
		_bobber.transform.localPosition = Vector3.zero;
		_bobber.transform.rotation = Quaternion.Euler(Vector3.zero);
		FishGameController.Instance.BobberInLake = false;
		FishGameController.Instance.FishIsBite = false;
	}
	
	public void CoughtFish()
	{
		FishGameController.Instance.OnCoughtFish();
	}
}
