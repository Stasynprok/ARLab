using System;
using UnityEngine;

public class ReleaseFishMove : MonoBehaviour
{
	public Action OnRelease;
	
	public float AccelerationToRelease;
	public float Acceleration;
	private float _prevPosition;
	private bool _isInvoke;
	
	private void Start()
	{
		_prevPosition = transform.localRotation.x;
	}
	
	private void Update()
	{
		float cof = (transform.localRotation.x - _prevPosition) * 100f;
		
		Acceleration = cof / Time.deltaTime;
		
		_prevPosition = transform.localRotation.x;
		if(Acceleration < 0.1f)
		{
			_isInvoke = false;
		}
		
		if(AccelerationToRelease < Acceleration && !_isInvoke)
		{
			OnRelease?.Invoke();
			_isInvoke = true;
			Debug.LogWarning("OnRelease");
		}
	}
}
