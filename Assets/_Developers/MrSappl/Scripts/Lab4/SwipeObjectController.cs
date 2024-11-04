using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeObjectController : MonoBehaviour
{
    [SerializeField] private float _force = 10f;
    private Vector3 _rayDir = Vector3.up;
    private bool _enabled = false;
    private void OnEnable()
    {
        LeanTouch.OnFingerSwipe += OnSwipe;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= OnSwipe;
    }

    public void Activate()
    {
        _enabled = true;
    }
    public void Deactivate()
    {
        _enabled = true;
    }

    private void OnSwipe(LeanFinger finger)
    {
        if (!_enabled)
        {
            return;
        }

        Vector3 startPosition = InteractionManager.Instance.ARCamera.transform.TransformPoint(finger.StartScreenPosition);
        Vector3 finalPosition = InteractionManager.Instance.ARCamera.transform.TransformPoint(finger.ScreenPosition);
        _rayDir = finalPosition - startPosition;

        AddForceObjectOnSwipe(_rayDir.normalized, _force);
        Debug.Log($"swipe happened! Swipe distance = {finger.SwipeScreenDelta.magnitude}");
        Debug.Log($"starting position = {finger.StartScreenPosition}");
        Debug.Log($"final position = {finger.ScreenPosition}");
    }

    private void AddForceObjectOnSwipe(Vector3 direction, float force)
    {
        ObjectsOnScene.Instance.AddForceAllObjects(direction, force);
    }
}
