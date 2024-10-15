using LAB3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class ObjectControllerSingletone : MonoBehaviour
{
    public Dictionary<int, CreatedObject> CreatedObjects = new Dictionary<int, CreatedObject>();

    public static ObjectControllerSingletone Instance = null;

    private int _keyLastSelectedObject = -1;

    private PlaneController _aRPlaneController;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance == this)
        {
            Destroy(gameObject);
        }

        _aRPlaneController = GetComponent<PlaneController>();
    }

    public void AddCreatedObject(CreatedObject createdObject)
    {
        int objectID = createdObject.Number;
        if (CreatedObjects.ContainsKey(objectID))
        {
            return;
        }
        CreatedObjects.Add(objectID, createdObject);
    }

    public void OnSelectObject(int key)
    {
        if (!CreatedObjects.ContainsKey(key))
        {
            Debug.LogError("Nety");
        }

        _keyLastSelectedObject = key;
        Component target = CreatedObjects[key].gameObject.transform;
        foreach (var createdObject in CreatedObjects)
        {
            if (createdObject.Key == key)
            {
                createdObject.Value.ChangeMaterialOnUnselectAnother();
                createdObject.Value.ActivateParticlesField();
                continue;
            }
            createdObject.Value.ChangeMaterealOnSelectAnother();
            createdObject.Value.ActivateParticles(target);
            _aRPlaneController.OnSelectObject();
        }
    }

    public void UnSelectAll()
    {
        if (!CreatedObjects.ContainsKey(_keyLastSelectedObject))
        {
            Debug.LogError("Nety");
        }

        foreach (var createdObject in CreatedObjects)
        {
            if (createdObject.Key == _keyLastSelectedObject)
            {
                continue;
            }
            createdObject.Value.ChangeMaterialOnUnselectAnother();
            createdObject.Value.DisableParticles();
        }

        _aRPlaneController.OnUnselectObject();
    }
}
