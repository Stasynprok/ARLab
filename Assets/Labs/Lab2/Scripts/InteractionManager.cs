using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace LAB2
{
    [RequireComponent(typeof(ARRaycastManager))]
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnedObjectPrefab;
        [SerializeField] private GameObject _targetMarkerPrefab;

        private ARRaycastManager _raycastManager;
        private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
        private GameObject _targetMarker;

        private void Awake()
        {
            _raycastManager = GetComponent<ARRaycastManager>();
        }

        private void Start()
        {
            _targetMarker = Instantiate(_targetMarkerPrefab, Vector3.zero, _targetMarkerPrefab.transform.rotation);
            _targetMarker.SetActive(false);
        }

        private void Update()
        {
            if (Input.touchCount == 0)
                return;

            ProcessFirstTouch(Input.GetTouch(0));
        }

        private void ProcessFirstTouch(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                ShowMarker(true);
                MoveMarker(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                MoveMarker(touch.position);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                SpawnObject(touch);
                ShowMarker(false);
            }
        }

        private void ShowMarker(bool value)
        {
            _targetMarker.SetActive(value);
        }

        private void MoveMarker(Vector2 touchPosition)
        {
            _raycastManager.Raycast(touchPosition, _raycastHits, TrackableType.Planes);
            _targetMarker.transform.position = _raycastHits[0].pose.position;
        }
        private void SpawnObject(Touch touch)
        {
            _raycastManager.Raycast(touch.position, _raycastHits, TrackableType.Planes);
            Instantiate(_spawnedObjectPrefab, _raycastHits[0].pose.position, _spawnedObjectPrefab.transform.rotation);
        }
    }
}
