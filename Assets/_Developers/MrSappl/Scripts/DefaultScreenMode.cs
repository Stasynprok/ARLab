using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class DefaultScreenMode : MonoBehaviour, IInteractionManagerMode
{
    [SerializeField] private GameObject _ui;

    private void OnEnable()
    {
        LeanTouch.OnFingerSwipe += OnSwipe;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= OnSwipe;
    }

    private void OnSwipe(LeanFinger finger)
    {
        Debug.Log($"swipe happened! Swipe distance = {finger.SwipeScreenDelta.magnitude}");
        Debug.Log($"starting position = {finger.StartScreenPosition}");
        Debug.Log($"final position = {finger.ScreenPosition}");
    }

    public void Activate()
    {
        _ui.SetActive(true);
    }

    public void Deactivate()
    {
        _ui.SetActive(false);
    }

    public void TouchInteraction(Touch[] touches)
    {
        return;
    }

    public void SelectMode(int mode)
    {
        InteractionManager.Instance.SelectMode(mode);
    }
}
