using UnityEngine;
using UnityEngine.UI;

public class FishingGameMode : MonoBehaviour, IInteractionManagerMode
{
	[Header("UI settings")]
	[SerializeField] private GameObject _ui;
	[Header("Target settings")]
	[SerializeField] private GameObject _targetMarkerPrefab;
	private GameObject _targetMarker;
	[Header("Game settings")]
	[SerializeField] private string _lakeTag;
	[SerializeField] private FishingRodController _fishingRodController;
	[SerializeField] private Button _returnBobberButton;
	
	private bool _isBobberLaunched = false;
	

	private void Start()
	{
		// create target marker
		_targetMarker = Instantiate(
			original: _targetMarkerPrefab,
			position: Vector3.zero,
			rotation: _targetMarkerPrefab.transform.rotation
		);
		_targetMarker.SetActive(false);
	}
	
	public void Activate()
	{
		_ui.SetActive(true);
		_fishingRodController.Activate();
		_returnBobberButton.onClick.AddListener(ReturnBobber);
	}
	
	private void ReturnBobber()
	{
		_isBobberLaunched = false;
		_fishingRodController.RaiseBobber();
	}
	
	public void Deactivate()
	{
		_ui.SetActive(false);
		_fishingRodController.Deactivate();
		_returnBobberButton.onClick.RemoveListener(ReturnBobber);
	}
	
	public void TouchInteraction(Touch[] touches)
	{
		if(_isBobberLaunched)
		{
			return;
		}
		
		Touch touch = touches[0];
		bool overUI = touch.position.IsPointOverUIObject();

		if (touch.phase == TouchPhase.Began)
		{
			if (!overUI)
			{
				ShowMarker(true);
				MoveMarker(touch.position);
			}
		}
		else if (touch.phase == TouchPhase.Moved)
		{
			if (_targetMarker.activeSelf)
				MoveMarker(touch.position);
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			if (_targetMarker.activeSelf)
			{
				LaunchBobber(touch.position);
				ShowMarker(false);
			}
		}
	}
	
	private void LaunchBobber(Vector2 touchWindowPosition)
	{
		Vector3 globalPositionBobber = TrySelectObject(touchWindowPosition);
		
		if(globalPositionBobber != Vector3.zero)
		{
			_isBobberLaunched = true;
			_fishingRodController.LaunchBobber(globalPositionBobber);
		}
	}
	
	private void ShowMarker(bool value)
	{
		_targetMarker.SetActive(value);
	}

	private void MoveMarker(Vector2 touchPosition)
	{
		_targetMarker.transform.position = TrySelectObject(touchPosition);
	}
	
	private Vector3 TrySelectObject(Vector2 pos)
	{
		Ray ray = InteractionManager.Instance.ARCamera.ScreenPointToRay(pos);
		RaycastHit hitObject;
		
		if (!Physics.Raycast(ray, out hitObject))
		{
			return Vector3.zero;
		}

		if (!hitObject.collider.CompareTag(_lakeTag))
		{
			return Vector3.zero;
		}

		return hitObject.point;
	}
}
