using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ObjectCreationMode : MonoBehaviour, IInteractionManagerMode
{
    [SerializeField] private GameObject[] _spawnedObjectPrefabs;
    [SerializeField] private GameObject _ui;
    [SerializeField] private GameObject _targetMarkerPrefab;

    private int _spawnedObjectType = -1;
    private int _spawnedObjectCount = 0;
    private GameObject _targetMarker;

    private Vector3 _positionTarget;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_positionTarget, 0.1f);
    }

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
        _spawnedObjectType = -1;
    }

    public void Deactivate()
    {
        _ui.SetActive(false);
        _spawnedObjectType = -1;
    }

    public void BackToDefaultScreen()
    {
        InteractionManager.Instance.SelectMode(0);
    }

    public void SetSpawnedObjectType(int spawnedObjectType)
    {
        _spawnedObjectType = spawnedObjectType;
    }

    public void TouchInteraction(Touch[] touches)
    {
        // if none are yet selected, return
        if (_spawnedObjectType == -1)
            return;

        Touch touch = touches[0];
        bool overUI = Vector2Extensions.IsPointOverUIObject(touch.position);

        if (overUI)
            return;

        if (touch.phase == TouchPhase.Began)
        {
            if (!overUI)
            {
                Debug.LogWarning("[DEBUGA] TouchPhase.Began");
                ShowMarker(true);
                MoveMarker(touch.position);
                _positionTarget = GetPositionOnRaycast(touch.position);
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
                Debug.LogWarning("[DEBUGA] TouchPhase.Ended");
                SpawnObject(touch);
                ShowMarker(false);
            }
        }
    }
    private void ShowMarker(bool value)
    {
        _targetMarker.SetActive(value);
    }

    private void MoveMarker(Vector2 touchPosition)
    {
        _targetMarker.transform.position = InteractionManager.Instance.GetARRaycastHits(touchPosition)[0].pose.position;
    }

    private void SpawnObject(Touch touch)
    {
        /*GameObject newObject = Instantiate(
            original: _spawnedObjectPrefabs[_spawnedObjectType],
            position: InteractionManager.Instance.GetARRaycastHits(touch.position)[0].pose.position,
            rotation: _spawnedObjectPrefabs[_spawnedObjectType].transform.rotation
        );*/

        Vector3 positionSpawn = GetPositionOnRaycast(touch.position);

        if (positionSpawn == Vector3.zero)
        {
            return;
        }

        GameObject newObject = Instantiate(
            original: _spawnedObjectPrefabs[_spawnedObjectType],
            position: positionSpawn,
            rotation: _spawnedObjectPrefabs[_spawnedObjectType].transform.rotation
        );

        CreatedObject objectDiscription = newObject.GetComponent<CreatedObject>();
        if (!objectDiscription)
        {
            Debug.LogError($"[ObjectCreationMode] {newObject.name} missing CreatedObject!");
            return;
        }
        objectDiscription.GiveNumber(++_spawnedObjectCount);
        ObjectsOnScene.Instance.AddNewObject(objectDiscription);
    }

    private Vector3 GetPositionOnRaycast(Vector2 startRay)
    {
        Ray ray = InteractionManager.Instance.ARCamera.ScreenPointToRay(startRay);
        RaycastHit hit;
        Vector3 position = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag != "AR")
            {
                Debug.Log("DOntFind" + hit.collider.tag);
                return position;
            }
            position = hit.point + hit.normal * 0.1f;
            Debug.Log(position);
        }
        return position;
    }
}
