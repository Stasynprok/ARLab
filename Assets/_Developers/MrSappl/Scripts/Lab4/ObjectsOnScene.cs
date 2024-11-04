using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsOnScene : MonoBehaviour
{
    public List<CreatedObject> CreatedObjects = new List<CreatedObject>();

    #region Singleton
    /// <summary>
    /// Instance of our Singleton
    /// </summary>
    public static ObjectsOnScene Instance
    {
        get
        {
            return _instance;
        }
    }
    private static ObjectsOnScene _instance;

    public void InitializeSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    #endregion

    private void Awake()
    {
        InitializeSingleton();
    }

    public void AddNewObject(CreatedObject createdObject)
    {
        CreatedObjects.Add(createdObject);
    }

    public void AddForceAllObjects(Vector3 direction, float force)
    {
        foreach (CreatedObject createdObject in CreatedObjects)
        {
            createdObject.AddForceToDirection(direction, force);
        }
    }

    public void AddExplosion(float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (CreatedObject createdObject in CreatedObjects)
        {
            createdObject.AddExplosion(explosionForce, explosionPosition, explosionRadius);
        }
    }

    public void FastMoveAtPosition(Vector3 target)
    {
        foreach (CreatedObject createdObject in CreatedObjects)
        {
            createdObject.FastMoveToTarget(target);
        }
    }
}
