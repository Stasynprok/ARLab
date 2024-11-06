using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Unity.Collections;
using System.Drawing;
using UnityEngine.XR.ARSubsystems;
using System;

public class SpawnInPolygon : MonoBehaviour
{
    public event Action NeedFindClear;
    [SerializeField] private int _maxCountObjects;
    [SerializeField] private int _countNeedFindObjects;
    [SerializeField] private float _offsetObjects;
    [SerializeField] private List<GameObject> _prefabsObjectsForCreate = new List<GameObject>();
    public bool Activate = false;

    private List<ObjectForGame> _objectsOnScene = new List<ObjectForGame>();
    private List<ObjectForGame> _objectsOnSceneNeedFind = new List<ObjectForGame>();
    private List<Vector3> _pointsObjects = new List<Vector3>();


    private void OnValidate()
    {
        if (Activate)
        {
            DestroyAllobjects();
            GenerateObjects();
            Activate = false;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 poin in _pointsObjects)
        {
            Gizmos.DrawSphere(poin, 0.1f);
        }
    }

    public void GenerateObjects()
    {
        DestroyAllobjects();
        GeneratePointsForObjects();
        GenerateObjectsOnPoints();
    }

    public void DestroyAllobjects()
    {
        if (_objectsOnScene.Count != 0)
        {
            foreach (ObjectForGame obj in _objectsOnScene)
            {
                Destroy(obj.gameObject);
            }
            _objectsOnScene.Clear();
        }

        if (_objectsOnSceneNeedFind.Count != 0)
        {
            foreach (ObjectForGame obj in _objectsOnSceneNeedFind)
            {
                Destroy(obj.gameObject);
            }
            _objectsOnSceneNeedFind.Clear();
        }
    }

    private void GenerateObjectsOnPoints()
    {
        int indexObject = 0;
        foreach (Vector3 point in _pointsObjects)
        {
            GameObject randomPrefab = GetRandomPrefab();
            GameObject createdObject = Instantiate(randomPrefab, point, RandomRotation());

            ObjectForGame objectForGame = createdObject.GetComponent<ObjectForGame>();
            objectForGame.Index = indexObject;
            indexObject++;
            if (_objectsOnSceneNeedFind.Count != _countNeedFindObjects)
            {
                objectForGame.NeedFindThis();
                _objectsOnSceneNeedFind.Add(objectForGame);
                continue;
            }
            objectForGame.DisableSign();
            _objectsOnScene.Add(objectForGame);
        }
    }

    private Quaternion RandomRotation()
    {
        float yValue = UnityEngine.Random.Range(-360, 360);

        Quaternion rotation = Quaternion.Euler(0.0f, yValue, 0.0f);
        return rotation;
    }

    private GameObject GetRandomPrefab()
    {
        int indexObject = UnityEngine.Random.Range(0, _prefabsObjectsForCreate.Count);

        return _prefabsObjectsForCreate[indexObject];
    }
    private void GeneratePointsForObjects()
    {
        _pointsObjects.Clear();

        int countObjects = UnityEngine.Random.Range(4, _maxCountObjects + 1);

        TrackableCollection<ARPlane> allPlanes = InteractionManager.Instance.GetPlanes();

        while (countObjects != 0)
        {
            foreach (ARPlane plane in allPlanes)
            {
                List<Vector2> points = new List<Vector2>();
                Transform planeTransform = transform;
                Vector3 planePosition = Vector3.zero;

                planeTransform = plane.transform;
                planePosition = plane.transform.position;
                foreach (Vector2 boundary in plane.boundary)
                {
                    points.Add(boundary);
                }

                Vector2 point;

                Vector3 pointInWorld;
                Vector3 point3D = Vector3.zero;

                do 
                {
                    point = GetRandomPointInPolygon(points);
                    pointInWorld = planeTransform.TransformPoint(new Vector3(point.x, 0, point.y));
                    point3D = new Vector3(pointInWorld.x, planePosition.y, pointInWorld.z);
                }
                while (!CloseByOffset(point3D));

                
                _pointsObjects.Add(point3D);

                countObjects--;
                if (countObjects <= 0)
                {
                    break;
                }
            }
        }
    }
    private Vector3 GetRandomPointInPolygon(List<Vector2> polygon)
    {
        Vector2 min = polygon[0];
        Vector2 max = polygon[0];

        foreach (var point in polygon)
        {
            if (point.x < min.x) min.x = point.x;
            if (point.y < min.y) min.y = point.y;
            if (point.x > max.x) max.x = point.x;
            if (point.y > max.y) max.y = point.y;
        }

        Vector2 randomPoint;
        int attempts = 0;

        do
        {
            float randomX = UnityEngine.Random.Range(min.x, max.x);
            float randomY = UnityEngine.Random.Range(min.y, max.y);
            randomPoint = new Vector2(randomX, randomY);
            attempts++;
        }
        while (!IsPointInPolygon(randomPoint, polygon) && attempts < 100);

        

        return randomPoint;
    }

    private bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        int j = polygon.Count - 1;
        bool inside = false;

        for (int i = 0; i < polygon.Count; j = i++)
        {
            if ((polygon[i].y > point.y) != (polygon[j].y > point.y) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
            {
                inside = !inside;
            }
        }

        return inside;
    }

    private bool CloseByOffset(Vector3 pointGenerated)
    {
        if (pointGenerated == Vector3.zero)
        {
            return false;
        }

        foreach (Vector3 pointObject in _pointsObjects)
        {
            if (Vector3.Distance(pointGenerated, pointObject) < _offsetObjects)
            {
                return false;
            }
        }
        return true;
    }

    public void GetSelectedObject(ObjectForGame objectForGame)
    {
        ObjectForGame result = null;
        foreach (ObjectForGame forGame in _objectsOnSceneNeedFind)
        {
            if (forGame.Index == objectForGame.Index)
            {
                result = forGame;
                break;
            }
        }

        if (result == null)
        {
            return;
        }

        if (result.ImFind())
        {
            Destroy(result.gameObject);
            _objectsOnSceneNeedFind.Remove(result);
            CheckClearNeedFindObjects();
        }
    }

    private void CheckClearNeedFindObjects()
    {
        if (_objectsOnSceneNeedFind.Count == 0)
        {
            NeedFindClear?.Invoke();
        }
    }
}
