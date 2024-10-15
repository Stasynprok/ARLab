using LAB2_ZIP;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LAB3 
{
    public class ObjectSelectionMode : MonoBehaviour, IInteractionManagerMode
    {
        [SerializeField] private GameObject _ui;
        [SerializeField] private GameObject _descriptionPanel;
        [SerializeField] private TMP_Text _objectTitleText;
        [SerializeField] private TMP_Text _objectDescriptionText;

        public void Activate()
        {
            _ui.SetActive(true);
            _descriptionPanel.SetActive(false);
        }

        public void Deactivate()
        {
            _descriptionPanel.SetActive(false);
            _ui.SetActive(false);
        }

        public void BackToDefaultScreen()
        {
            InteractionManager.Instance.SelectMode(0);

            //Added
            ObjectControllerSingletone.Instance.UnSelectAll();
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

            //Added
            OnSelectObject(objectDescription.Number);
        }

        private void ShowObjectDescription(CreatedObject targetObject)
        {
            _objectTitleText.text = targetObject.Name;
            _objectDescriptionText.text = targetObject.Description;
            _descriptionPanel.SetActive(true);
        }

        //Added
        private void OnSelectObject(int key)
        {
            ObjectControllerSingletone.Instance.OnSelectObject(key);
        }
    }
}

