using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace LAB1
{
    [RequireComponent(typeof(ARRaycastManager))]
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnedObjectPrefab;

        private ARRaycastManager _raycastManager;
        private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();

        private void Awake()
        {
            _raycastManager = GetComponent<ARRaycastManager>();
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
                SpawnObject(touch);
            }
        }

        private void SpawnObject(Touch touch)
        {
            _raycastManager.Raycast(touch.position, _raycastHits, TrackableType.Planes);
            Instantiate(_spawnedObjectPrefab, _raycastHits[0].pose.position, _spawnedObjectPrefab.transform.rotation);
        }
    }

}
