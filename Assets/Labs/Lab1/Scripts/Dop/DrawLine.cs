using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    [SerializeField] private float _minDistance;

    private ARRaycastManager _raycastManager;
    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();

    private LineRenderer _lineRenderer;
    private Vector3 _previousPosition;
    
    private Vector2 _centerScreen;

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _lineRenderer = GetComponent<LineRenderer>();
        _centerScreen = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        DrawLineOnTouch(Input.GetTouch(0));
    }

    private void DrawLineOnTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            _lineRenderer.positionCount = 0;
        }

        if (touch.phase == TouchPhase.Stationary)
        {
            _raycastManager.Raycast(_centerScreen, _raycastHits, TrackableType.Planes);

            if (_raycastHits.Count == 0)
            {
                return;
            }

            Vector3 currentPosition = _raycastHits[0].pose.position;

            if (_lineRenderer.positionCount == 0 || Vector3.Distance(currentPosition, _previousPosition) > _minDistance)
            {
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, currentPosition);
                _previousPosition = currentPosition;
            }
        }
    }
}
