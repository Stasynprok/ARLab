using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace LAB2DOP
{
    public class DrawCurvedLine : MonoBehaviour, IInteractionManagerMode
    {
        [SerializeField] private float _minDistance;
        [SerializeField] private GameObject _uiLineController;
        [SerializeField] private GameObject _parentForLine;
        [SerializeField] private GameObject _prefabLine;
        private bool _active = false;

        private ARRaycastManager _raycastManager;
        private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
        private Vector3 _previousPosition;

        private Vector2 _centerScreen;

        private List<LineRenderer> _lines = new List<LineRenderer>();
        private LineRenderer _currentLine;

        private void Awake()
        {
            _raycastManager = GetComponent<ARRaycastManager>();
            _centerScreen = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        }

        /*private void Update()
        {
            *//*if (Input.touchCount == 0 || Vector2Extensions.IsPointOverUIObject())
            {
                return;
            }*//*
            if (_active)
            {
                return;
            }

            if (Input.touchCount == 0)
            {
                return;
            }

            TouchInterpretation(Input.GetTouch(0));
        }*/

        private void Start()
        {
            InteractionManager.Instance.ClearAll += DestroyAllLines;
        }

        private void TouchInterpretation(Touch touch)
        {
            if (_active)
            {
                return;
            }
            if (touch.phase == TouchPhase.Began)
            {
                CreateLine();
            }

            if (touch.phase == TouchPhase.Stationary)
            {
                DrawLineOnTouch();
            }
        }
        private void CreateLine()
        {
            GameObject newLineObject = Instantiate(_prefabLine, _parentForLine.transform);
            LineRenderer line = newLineObject.GetComponent<LineRenderer>();
            _currentLine = line;
            _lines.Add(line);
        }

        private void DestroyAllLines()
        {
            foreach (LineRenderer line in _lines)
            {
                Destroy(line.gameObject);
            }
            _lines.Clear();
            _currentLine = null;
        }

        private void DrawLineOnTouch()
        {
            if (!_currentLine)
            {
                return;
            }
            _raycastManager.Raycast(_centerScreen, _raycastHits, TrackableType.Planes);

            if (_raycastHits.Count == 0)
            {
                return;
            }

            Vector3 currentPosition = _raycastHits[0].pose.position;

            if (_currentLine.positionCount == 0 || Vector3.Distance(currentPosition, _previousPosition) > _minDistance)
            {
                _currentLine.positionCount++;
                _currentLine.SetPosition(_currentLine.positionCount - 1, currentPosition);
                _previousPosition = currentPosition;
            }
        }

        public void Activate()
        {
            _uiLineController.SetActive(true);
            _active = true;
        }

        public void Deactivate()
        {
            _uiLineController.SetActive(false);
            _active = false;
        }

        public void TouchInteraction(Touch[] touches)
        {
            TouchInterpretation(touches[0]);
        }
    }
}
