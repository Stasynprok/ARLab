using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSelectionMode : MonoBehaviour, IInteractionManagerMode
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private GameObject _descriptionPanel;
    [SerializeField] private TMP_Text _objectTitleText;
    [SerializeField] private TMP_Text _objectDescriptionText;
    [SerializeField] private SwipeObjectController _swipeObjectController;
    [SerializeField] private TapController _tapController;

    public void Activate()
    {
        _ui.SetActive(true);
        _descriptionPanel.SetActive(false);

        _swipeObjectController.Activate();
        _tapController.ActivateDoubleTap = true;
    }

    public void Deactivate()
    {
        _descriptionPanel.SetActive(false);
        _ui.SetActive(false);

        _swipeObjectController.Deactivate();
        _tapController.ActivateDoubleTap = false;
    }

    public void BackToDefaultScreen()
    {
        InteractionManager.Instance.SelectMode(0);
    }

    public void TouchInteraction(Touch[] touches)
    {
        Touch touch = touches[0];
        bool overUI = touch.position.IsPointOverUIObject();

        if (touch.phase == UnityEngine.TouchPhase.Began)
        {
            if (!overUI)
            {
                TrySelectObject(touch.position);
            }
        }
        if (touches.Length > 1)
        {
            MoveObjects(touch, touches[1]);
        }
    }

    private void TrySelectObject(Vector2 pos)
    {
        // fire a ray from camera to the target screen position
        Ray ray = InteractionManager.Instance.ARCamera.ScreenPointToRay(pos);
        RaycastHit hitObject;
        if (!Physics.Raycast(ray, out hitObject))
            return;

        if (!hitObject.collider.CompareTag("CreatedObject"))
            return;

        // if we hit a spawned object tag, try to get info from it
        GameObject selectedObject = hitObject.collider.gameObject;
        CreatedObject objectDescription = selectedObject.GetComponent<CreatedObject>();
        if (!objectDescription)
            throw new MissingComponentException("[OBJECT_SELECTION_MODE] " + selectedObject.name + " has no description!");

        ShowObjectDescription(objectDescription);
    }

    private void ShowObjectDescription(CreatedObject targetObject)
    {
        _objectTitleText.text = targetObject.Name;
        _objectDescriptionText.text = targetObject.Description;
        _descriptionPanel.SetActive(true);
    }

    private void MoveObjects(Touch touch1, Touch touch2)
    {
        if (touch1.phase == UnityEngine.TouchPhase.Moved || touch2.phase == UnityEngine.TouchPhase.Moved)
        {
            float distance = Vector2.Distance(touch1.position, touch2.position);
            float distancePrev = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
            float delta = distance - distancePrev;

            if ((Mathf.Sign(delta) * 1) < 0)
            {
                Vector3 centerBetweenTouch = Vector3.Lerp(touch1.position, touch2.position, 0.5f);

                Vector3 center = GetPositionOnRaycast(centerBetweenTouch);

                if (center != Vector3.zero)
                {
                    ObjectsOnScene.Instance.FastMoveAtPosition(center);
                }
            }
        }
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

